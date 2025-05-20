import React, { useEffect, useState,  useContext, useRef} from 'react';
import { useParams } from 'react-router-dom';
import axios from '../../api/axios';
import { AuthContext } from '../../context/AuthProvider';
import LinkButton from '../../components/LinkButton';
import APIs from '../../api/ApiURL';


const CustomerDetails = () => {
    const { customerId } = useParams();
    const { userId } = useContext(AuthContext);
    const [detail, setCustomers] = useState({});

    const fetchDetails = async () => {
          try {
            const response = await axios.get(`${APIs.CUSTOMER_DETAILS}/${customerId}`,{
                headers: {
                    'Content-Type': 'application/json',
                    'auth': userId
                }
            });
            if(response.status === 200){
                console.log(response.data);
                setCustomers(response.data)
            }
            
          } catch (err) {
            popUpRef.current?.show('Błąd pobierania użytkowników: ' + err.message);
          }
        };
    
        useEffect(() => {
          fetchDetails();
      }, []);

  return (
    <div className='contentColumn'>
    <div id='userInfo' style={{'width': '100%'}}>
        <div><span className='highlight'>Name: </span>{detail.nameCustomer}</div>
        <div ><span className='highlight'>Surname: </span>{detail.surnameCustomer}</div>
        <div ><span className='highlight'>Phone number: </span>{detail.phoneNumber}</div>
    </div>
    <div style={{'width': '100%'}}>
    <table className='dataTable' >
            <thead>
            <tr className='dataTr'>
                <th className='dataTh'>Image</th>
                <th className='dataTh'>Info</th>
                <th></th>
            </tr>
            </thead>
            <tbody>

            </tbody>
        </table>
    </div>
     
    </div>
  )
}

export default CustomerDetails