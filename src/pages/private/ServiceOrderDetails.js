import React, { useEffect, useState, useContext, useRef } from 'react';
import { useLocation } from 'react-router-dom';
import axios from '../../api/axios';
import APIs from '../../api/ApiURL';
import { AuthContext } from '../../context/AuthProvider';
import PopUp from '../../components/PopUp';
import LinkButton from '../../components/LinkButton';

const ServiceOrderDetails = () => {
  const popUpRef = useRef();
  const location = useLocation();
  const { userId, role } = useContext(AuthContext);
  const so = location.state;
  const [statusOrder, setStatusOrder] = useState(so.statusOrder);
  const [serviceTasks, setServiceTasks] = useState(
    [
      {
        serviceTaskId: 1,
        name: "Wymiana oleju silnikowego",
        laborCost: 100.00,
        part: {
          namePart: "Olej syntetyczny",
          typePart: "5W30"
        },
        quantity: 1,
        totalCost: 100.00  // laborCost + (partPrice * quantity), np. część gratis lub wewnętrznie rozliczona
      },
      {
        serviceTaskId: 2,
        name: "Wymiana filtra powietrza",
        laborCost: 50.00,
        part: {
          namePart: "Filtr powietrza",
          typePart: "OP1000"
        },
        quantity: 1,
        totalCost: 70.00  // np. laborCost 50 + partCost 20
      },
      {
        serviceTaskId: 3,
        name: "Wymiana klocków hamulcowych",
        laborCost: 120.00,
        part: {
          namePart: "Klocki hamulcowe",
          typePart: "KH-200"
        },
        quantity: 4,
        totalCost: 400.00  // laborCost 120 + (partCost 70 * 4) = 120 + 280 = 400
      },
      {
        serviceTaskId: 4,
        name: "Diagnostyka silnika",
        laborCost: 80.00,
        part: null,       // w tym przypadku nie używamy żadnej części
        quantity: 0,
        totalCost: 80.00  // tylko koszt robocizny
      },
      {
        serviceTaskId: 5,
        name: "Wymiana akumulatora",
        laborCost: 90.00,
        part: {
          namePart: "Akumulator",
          typePart: "60Ah"
        },
        quantity: 1,
        totalCost: 190.00 // laborCost 90 + partCost 100 = 190
      }
    ]
  );


  console.log(so);

  const fetchServiceTasks = async () => {
    try {
      const ServiceOrderId = so.serviceOrderId;
      const response = await axios.get(`${APIs.GET_MECHANICS_TASKS}/${ServiceOrderId}`, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log(response.data);
        setServiceTasks(response.data);
      }

    } catch (err) {
      popUpRef.current?.show(err.response.data || err.message);
    }
  };

  const setStatus = async (status) => {
    try {
      const ServiceOrderId = so.serviceOrderId;
      const response = await axios.put(`${APIs.SET_STATUS}/${ServiceOrderId}`,
        JSON.stringify({
          Status: status
        }),
        {
          headers: {
            'Content-Type': 'application/json'
          },
          withCredentials: true
        });
      if (response.status === 200) {
        console.log(response.data);
        popUpRef.current?.show(response.data.message);
        setStatusOrder(status);
      }

    } catch (err) {
      popUpRef.current?.show(err.response.data || err.message);
    }
  }


  useEffect(() => {
    fetchServiceTasks();
  }, []);


  return (
    <div className='contentColumn'>

      <div id='userInfo2'>
        <div><span className='highlight'>Service task for </span></div>
        <div><span className='highlight'>Name and Surname: </span>{so.customer.nameCustomer} {so.customer.surnameCustomer}</div>
        <div ><span className='highlight'>Car info: </span>{so.vehicle.brandVehicle} {so.vehicle.modelVehicle} {so.vehicle.registralNumberVehicle}</div>
        <div ><span className='highlight'>Description: </span>{so.description}</div>
        <div ><span className='highlight'>Mechanic: </span>{so.mechanic?.userName}</div>
        <div ><span className='highlight'>Status: </span>{statusOrder}</div>
      </div>
      <div id='btnLayout'>
        { // tylko przypisany mechanik lub admin może zmienić status | tylko mechanik może dodać taska
          ["Nowe", "W trakcie"].includes(statusOrder) && (userId === so.mechanic?.id || role === "admin") && (
            <>
              {userId === so.mechanic?.id && (
                <LinkButton webpath={`/addservicetask`} name='Add Service Task' stateObj={so} />
              )}
              <button className="navButton" onClick={() => setStatus("Zakonczone")}>Complete Order</button>
              <button className="navButton" onClick={() => setStatus("Anulowane")}>Cancel Order</button>
            </>
          )
        }
      </div>

      <PopUp ref={popUpRef} />
      <div style={{ 'width': '100%' }}>
        <table className='dataTable'>
          <thead>
            <tr className='dataTr'>
              <th className='dataTh'>Nr.</th>
              <th className='dataTh'>Name</th>
              <th className='dataTh'>Labor Cost</th>
              <th className='dataTh'>Part</th>
              <th className='dataTh'>Quantity</th>
              <th className='dataTh'>Total Cost</th>
              <th className='dataTh'></th>
            </tr>
          </thead>
          <tbody>
            {serviceTasks != null && serviceTasks.map((st, index) => (
              <tr className='dataTr' key={st.serviceTaskId}>
                <td className='dataTd'>{index + 1}</td>
                <td className='dataTd'>{st.name}</td>
                <td className='dataTd'>{st.laborCost}</td>
                <td className='dataTd'>
                  {
                    st.part ?
                      `${st.part.namePart} ${st.part.typePart}`
                      :
                      userId == so.mechanic?.id ? //tylko przypisany mechanik może dodawać Part
                        <LinkButton webpath={`/addusedpart`} name='Add Part' stateObj={st} />
                        :
                        "-"
                  }
                </td>
                <td className='dataTd'>{st.quantity}</td>
                <td className='dataTd'>{st.totalCost}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

export default ServiceOrderDetails