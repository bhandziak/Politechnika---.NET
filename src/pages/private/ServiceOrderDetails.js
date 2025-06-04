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
  const [serviceTasks, setServiceTasks] = useState([]);


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
          status,
        {
          headers: {
            'Content-Type': 'application/json'
          },
          withCredentials: true
        });
      if (response.status === 200) {
        console.log(response.data);
        popUpRef.current?.show(response.data);
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
          ["Nowe", "WTrakcie"].includes(statusOrder) && (userId === so.mechanic?.id || role === "admin") && (
            <>
              {userId === so.mechanic?.id && (
                <LinkButton webpath={`/addservicetask`} name='Add Service Task' stateObj={so} />
              )}
              <button className="navButton" onClick={() => setStatus("Zakończone")}>Complete Order</button>
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
                    st?.usedPart ?
                      `${st.usedPart.part.namePart} ${st.usedPart.part.typePart}`
                      :
                      userId == so.mechanic?.id ? //tylko przypisany mechanik może dodawać Part
                        <LinkButton webpath={`/addusedpart`} name='Add Part' stateObj={st} />
                        :
                        "-"
                  }
                </td>
                <td className='dataTd'>{st?.usedPart ?  st.usedPart.quantity : "-"}</td>
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