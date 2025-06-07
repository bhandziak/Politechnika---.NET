import React, { useEffect, useState, useContext, useRef } from 'react';
import axios from '../../../api/axios';
import APIs from '../../../api/ApiURL';
import { AuthContext } from '../../../context/AuthProvider';
import PopUp from '../../../components/PopUp';
import LinkButton from '../../../components/LinkButton';

const CustomersPage = () => {
  const popUpRef = useRef();
  const { login, userId, role, setAuth } = useContext(AuthContext);
  const [customers, setCustomers] = useState([]);

  const months = ['select month ...', 'styczeń', 'luty', 'marzec', 'kwiecień', 'maj', 'czerwiec',
    'lipiec', 'sierpień', 'wrzesień', 'październik', 'listopad', 'grudzień'];
  const [monthSelect, setMonthSelect] = useState("");

  const handleChange = (e) => {
    popUpRef.current?.hide();
    const { name, value } = e.target;

    if (name == "monthSelect") {
      setMonthSelect(value);
    }
  }


  const downloadRaport = async (e) => {
    e.preventDefault();
    try {
      let monthsId = months.indexOf(monthSelect) - 1;
      const response = await axios.get(`${APIs.DOWNLOAD_RAPORT}/${monthsId}`, {
        responseType: 'blob',
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log("pobieranie raportu...");
        const blob = new Blob([response.data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `raport-${monthsId + 1}.pdf`);
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.URL.revokeObjectURL(url);
      }

    } catch (err) {
      popUpRef.current?.show("Brak raportu na dany miesiąc");
    }
  };

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
      {
        role == "admin" &&
        <div className='btnLayout'>
          < br />
          <form>
            <select
              className='additionalMargin'
              name="monthSelect"
              value={monthSelect}
              onChange={handleChange}
            >
              {months.map(m => (
                <option key={m} value={m}>{m}</option>
              ))}
            </select>

            <button className="btn additionalMargin" onClick={downloadRaport}>Download Raport</button>
          </form>
        </div>
      }

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