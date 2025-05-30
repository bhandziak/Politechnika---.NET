import React, { useEffect, useState,  useContext, useRef} from 'react';
import axios from '../../api/axios';
import APIs from '../../api/ApiURL';
import { AuthContext } from '../../context/AuthProvider';
import PopUp from '../../components/PopUp';
import LinkButton from '../../components/LinkButton';

const ServiceOrderPage = () => {
    const popUpRef = useRef();
    const { userId, role } = useContext(AuthContext);
    const [serviceOrders, setServiceOrders] = useState([
  {
    serviceOrderId: 1,
    customer: {
      nameCustomer: "Jan",
      surnameCustomer: "Kowalski"
    },
    vehicle: {
      brandVehicle: "Toyota",
      modelVehicle: "Corolla",
      registralNumberVehicle: "WX12345"
    },
    description: "Wymiana klocków hamulcowych",
    mechanic: "Marek Nowak",
    statusOrder: null
  },
  {
    serviceOrderId: 2,
    customer: {
      nameCustomer: "Anna",
      surnameCustomer: "Nowak"
    },
    vehicle: {
      brandVehicle: "Ford",
      modelVehicle: "Focus",
      registralNumberVehicle: "KR54321"
    },
    description: "Przegląd okresowy",
    mechanic: "Paweł Wiśniewski",
    statusOrder: 'Nowy'
  }
]);

    const fetchServiceOrders = async () => {
      try {
        const response = await axios.get(APIs.GET_ALL_SERVICE_ORDERS,{
            headers: {
                'Content-Type': 'application/json',
                'auth': userId
            }
        });
        if(response.status === 200){
            console.log(response.data)
            setServiceOrders(response.data);
        }

      } catch (err) {
        popUpRef.current?.show(err.response.data || err.message);
      }
    };

    useEffect(() => {
      fetchServiceOrders();
  }, []);

  return (
    <div className='contentColumn'>

        <PopUp ref={popUpRef} />
        <div style={{'width': '100%'}}>
      <table className='dataTable'>
        <thead>
          <tr className='dataTr'>
            <th className='dataTh'>Nr.</th>
            <th className='dataTh'>Name and Surname</th>
            <th className='dataTh'>Car info</th>
            <th className='dataTh'>Description</th>
            <th className='dataTh'>Mechanic</th>
            <th className='dataTh'>Status</th>
          </tr>
        </thead>
        <tbody>
          {serviceOrders != null && serviceOrders.map((so,index) => (
            <tr className='dataTr' key={so.serviceOrderId}>
              <td className='dataTd'>{index + 1}</td>
              <td className='dataTd'>{so.customer.nameCustomer} {so.customer.surnameCustomer}</td>
              <td className='dataTd'>{so.vehicle.brandVehicle} {so.vehicle.modelVehicle} {so.vehicle.registralNumberVehicle}</td>
              <td className='dataTd'>{so.description}</td>
              <td className='dataTd'>{so.mechanic}</td>
              <td className='dataTd'>
                {
                    so.statusOrder == null ?
                    <LinkButton webpath={`/addserviceorder`} name='Add Order' stateObj={so}/>
                    :
                    so.statusOrder
                }
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      </div>

    </div>
  )
}

export default ServiceOrderPage