import React, { useEffect, useState,  useContext, useRef} from 'react';
import { useParams } from 'react-router-dom';
import axios from '../../api/axios';
import { AuthContext } from '../../context/AuthProvider';
import LinkButton from '../../components/LinkButton';
import APIs from '../../api/ApiURL';
import PopUp from '../../components/PopUp';

const CustomerDetails = () => {
  const popUpRef = useRef();
  const { customerId } = useParams();
  const { userId, role } = useContext(AuthContext);
  const [detail, setCustomers] = useState({});

  const fetchDetails = async () => {
      try {
        const response = await axios.get(`${APIs.CUSTOMER_DETAILS}/${customerId}`,{
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true
        });
        if(response.status === 200){
            console.log(response.data);
            setCustomers(response.data)
        }
        
      } catch (err) {
        popUpRef.current?.show('Błąd pobierania użytkowników: ' + err.response.data);
      }
    };

    useEffect(() => {
      fetchDetails();
  }, []);

  const sendPhoto = async (file, vehicleId) => {
      try {
        const formData = new FormData();
        formData.append("photo", file);

        console.log("Id pojazdu", vehicleId)

        const response = await axios.post(
          `${APIs.SEND_PHOTO}/${vehicleId}`,
          formData,
          {
            headers: {},
            withCredentials: true
        });
        if(response.status === 200){
            console.log(response.data);
            fetchDetails();
        }
        
      } catch (err) {
        popUpRef.current?.show('Błąd przesyłania zdjęcia: ' + err.response.data);
      }
    };

  return (
    <div className='contentColumn'>
    <PopUp ref={popUpRef} />
    <div id='userInfo2'>
        <div><span className='highlight'>Name: </span>{detail.nameCustomer}</div>
        <div ><span className='highlight'>Surname: </span>{detail.surnameCustomer}</div>
        <div ><span className='highlight'>Phone number: </span>{detail.phoneNumber}</div>
        { ['receptionist', 'admin'].includes(role)  &&
          <LinkButton webpath='/addvehicle' name='Add vehicle' stateObj={detail} />
        }             
    </div>
    
    <div style={{'width': '100%'}}>
    <table className='dataTable' >
            <thead>
            <tr className='dataTr'>
                <th className='dataTh'>Nr.</th>
                <th className='dataTh'>Image</th>
                <th className='dataTh'>Info</th>  
            </tr>
            </thead>
            <tbody>
            {detail.vehicles != null && detail.vehicles.map((vehicle,index) => (
            <tr className='dataTr' key={vehicle.vehicleId}>
              <td className='dataTd'>{index + 1}</td>
              <td className='dataTd'>
                {vehicle.imageURL == "none" ?
                <>
                  <input
                    type="file"
                    accept="image/*"
                    onChange={(e) => {
                      const file = e.target.files[0];
                      if (file != null) sendPhoto(file, vehicle.vehicleId);
                    }}
                  />
                </>
                :
                <img  src={'https://localhost:7018' + vehicle.imageURL} alt="vehicle" style={{ width: '300px' }}/>
                }
              </td>
              <td className='dataTd'>
                <div><span className='highlight'>Brand: </span>{vehicle.brandVehicle}</div>
                <div><span className='highlight'>Model: </span>{vehicle.modelVehicle}</div>
                <div><span className='highlight'>VIN: </span>{vehicle.vinVehicle}</div>
                <div><span className='highlight'>Registral number: </span>{vehicle.registralNumberVehicle}</div>
                <div><span className='highlight'>Year: </span>{vehicle.yearVehicle}</div>
              </td>
            </tr>
          ))}
            </tbody>
        </table>
    </div>
                 
    </div>
  )
}

export default CustomerDetails