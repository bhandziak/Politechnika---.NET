import React, { useEffect, useState, useContext, useRef } from 'react';
import axios from '../../api/axios';
import APIs from '../../api/ApiURL';
import { AuthContext } from '../../context/AuthProvider';
import PopUp from '../../components/PopUp';
import LinkButton from '../../components/LinkButton';

const CustomersPage = () => {
  const popUpRef = useRef();
  const { login, userId, role, setAuth } = useContext(AuthContext);
  const [customers, setCustomers] = useState([]);

  const fetchCustomers = async () => {
    try {
      const response = await axios.get(APIs.GET_ALL_CUSTOMERS, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log(response.data)
        setCustomers(response.data);
      }

    } catch (err) {
      popUpRef.current?.show('Błąd pobierania użytkowników: ' + err.message);
    }
  };

  const deleteCustomer = async (customerId) => {
    try {
      const response = await axios.delete(`${APIs.DELETE_CUSTOMER}/${customerId}`, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log(response.data);
        popUpRef.current?.show(response.data);
        fetchCustomers();
      }

    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  }

  useEffect(() => {
    fetchCustomers();
  }, []);
  return (
    <div className='contentColumn'>
      <div id='userInfo2'>
        {['receptionist', 'admin'].includes(role) &&
          <LinkButton webpath='/addcustomer' name='Add customer' />
        }
      </div>


      <PopUp ref={popUpRef} />
      <div style={{ 'width': '100%' }}>
        <table className='dataTable'>
          <thead>
            <tr className='dataTr'>
              <th className='dataTh'>Nr.</th>
              <th className='dataTh'>Name</th>
              <th className='dataTh'>Surname</th>
              <th className='dataTh'>Phone number</th>
              <th></th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {customers != null && customers.map((customer, index) => (
              <tr className='dataTr' key={customer.customerId}>
                <td className='dataTd'>{index + 1}</td>
                <td className='dataTd'>{customer.nameCustomer}</td>
                <td className='dataTd'>{customer.surnameCustomer}</td>
                <td className='dataTd'>{customer.phoneNumber}</td>
                <td className='dataTd'>
                  <LinkButton webpath={`/details/${customer.customerId}`} name='Details'
                  cssClass={'detailsButton'}
                  />
                </td>
                {['receptionist', 'admin'].includes(role) &&
                  <>
                    <td>
                      <LinkButton webpath='/addcustomer' name='Update' stateObj={
                        { action: "update", customer: customer }
                      } 
                      cssClass={'updateButton'}
                      />
                    </td>
                    <td>
                      <button className="btn deleteButton" onClick={() => deleteCustomer(customer.customerId)}>Delete</button>
                    </td>
                  </>
                }
              </tr>
            ))}
          </tbody>
        </table>
      </div>


    </div>
  )
}

export default CustomersPage