import React, { useEffect, useState, useContext, useRef } from 'react';
import axios from '../../api/axios';
import APIs from '../../api/ApiURL';
import { AuthContext } from '../../context/AuthProvider';
import PopUp from '../../components/PopUp';
import LinkButton from '../../components/LinkButton';

const ServiceOrderPage = () => {
  const popUpRef = useRef();
  const { userId, role } = useContext(AuthContext);
  const [serviceOrders, setServiceOrders] = useState([]);
  const [API_URL, setAPI_URL] = useState(APIs.GET_ALL_SERVICE_ORDERS);
  const [currentBtnAction, setCurrentBtnAction] = useState("All Orders");

  const switchToMyOrders = () => {
    setAPI_URL(APIs.GET_MECHANICS_SERVICES);
    setCurrentBtnAction("My Orders");
  }

  const switchToAllOrders = () => {
    setAPI_URL(APIs.GET_ALL_SERVICE_ORDERS);
    setCurrentBtnAction("All Orders");
  }

  const fetchServiceOrders = async () => {
    try {
      const response = await axios.get(API_URL, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log("Services orders:");
        console.log(response.data)
        setServiceOrders(response.data);
      }

    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  };

  useEffect(() => {
    fetchServiceOrders();
  }, [API_URL]);

  return (
    <div className='contentColumn'>

      <PopUp ref={popUpRef} />
      <div style={{ 'width': '100%' }}>
        {
          role == "mechanic" ?
            <div id='btnLayout'>
              <button className={currentBtnAction == "My Orders" ? 'navButton navButtonActive' : 'navButton'} onClick={switchToMyOrders}>My Services</button>
              <button className={currentBtnAction == "All Orders" ? 'navButton navButtonActive' : 'navButton'} onClick={switchToAllOrders}>All Services</button>
            </div>
            :
            <></>
        }

        <table className='dataTable'>
          <thead>
            <tr className='dataTr'>
              <th className='dataTh'>Nr.</th>
              <th className='dataTh'>Name and Surname</th>
              <th className='dataTh'>Car info</th>
              <th className='dataTh'>Description</th>
              <th className='dataTh'>Mechanic</th>
              <th className='dataTh'>Status</th>
              <th className='dataTh'>Completion date</th>
              <th className='dataTh'></th>
            </tr>
          </thead>
          <tbody>
            {serviceOrders != null && serviceOrders.map((so, index) => (
              so.statusOrder != null || ['receptionist', 'admin'].includes(role) ?
                <tr className='dataTr' key={so.serviceOrderId}>
                  <td className='dataTd'>{index + 1}</td>
                  <td className='dataTd'>{so.customer.nameCustomer} {so.customer.surnameCustomer}</td>
                  <td className='dataTd'>{so.vehicle.brandVehicle} {so.vehicle.modelVehicle} {so.vehicle.registralNumberVehicle}</td>
                  <td className='dataTd'>{so.description}</td>
                  <td className='dataTd'>{so.mechanic?.userName}</td>
                  <td className='dataTd'>{so.statusOrder}</td>
                  <td className='dataTd'>{so.dateFinished != null ? so.dateFinished : "-"}</td>
                  <td className='dataTd'>
                    {
                      so.statusOrder != null ? // zlecenie stworzone
                        <LinkButton webpath={`/serviceorderdetails`} name='Details' stateObj={so} />
                        :  // jeszcze niestworzone zlecenie
                        ['receptionist', 'admin'].includes(role) ?
                          <LinkButton webpath={`/addserviceorder`} name='Add Order' stateObj={so} />
                          :
                          <></>
                    }
                  </td>
                </tr>
                :
                <></>
            ))}
          </tbody>
        </table>
      </div>

    </div>
  )
}

export default ServiceOrderPage