import React, { useEffect, useState, useContext, useRef } from 'react';
import { useLocation } from 'react-router-dom';
import axios from '../../../api/axios';
import APIs from '../../../api/ApiURL';
import { AuthContext } from '../../../context/AuthProvider';
import PopUp from '../../../components/PopUp';

const RaportPage = () => {
  const popUpRef = useRef();
  const location = useLocation();
  const { userId, role } = useContext(AuthContext);
  const customer = location.state;
  const customerId = customer.customerId;
  const [serviceOrders, setServiceOrders] = useState([]);
  const [serviceOrdersFiltered, setServiceOrdersFiltered] = useState([]);
  const [filterCarText, setFilterCarText] = useState("");

  const months = ['select month ...', 'styczeń', 'luty', 'marzec', 'kwiecień', 'maj', 'czerwiec',
    'lipiec', 'sierpień', 'wrzesień', 'październik', 'listopad', 'grudzień'];
  const [monthFilter, setMonthFilter] = useState("");

  const handleChange = (e) => {
    popUpRef.current?.hide();
    const { name, value } = e.target;

    let actualFilterCarText = filterCarText;
    let actualMonthFilter = monthFilter;

    // filtrowanie
    if (name == "filterCarText") {
      actualFilterCarText = value;
      setFilterCarText(value);

    } else if (name == "filterMonth") {
      actualMonthFilter = value;
      setMonthFilter(value);
    }

    const filteredOrders = serviceOrders.filter(so => {
      // car name match
      const vehicleName = `${so.vehicle.brandVehicle} ${so.vehicle.modelVehicle}`.toLowerCase();
      const vehicleMatch = vehicleName.includes(actualFilterCarText.toLowerCase());

      // month match
      const monthMatch = actualMonthFilter === "" || actualMonthFilter === "select month ..." || (
        so.dateFinished && new Date(so.dateFinished).getMonth() === months.indexOf(actualMonthFilter) - 1
      );

      return vehicleMatch && monthMatch;
    });

    setServiceOrdersFiltered(filteredOrders);

  };

  const fetchRaport = async () => {
    try {
      const response = await axios.get(`${APIs.GET_RAPORT}/${customerId}`, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log(response.data);
        setServiceOrders(response.data);
        setServiceOrdersFiltered(response.data);
      }

    } catch (err) {
      popUpRef.current?.show(err.response.data || err.message);
    }
  };

  useEffect(() => {
    fetchRaport();
  }, []);

  return (
    <div className='contentColumn'>

      <div id='userInfo2'>
        <div><span className='highlight'>Raport for</span></div>
        <div><span className='highlight'>Name: </span>{customer.nameCustomer}</div>
        <div ><span className='highlight'>Surname: </span>{customer.surnameCustomer}</div>
      </div>
      <PopUp ref={popUpRef} />

      <form>
        <br />
        <div id='btnLayout'>
          <label htmlFor="filterCarText">
            Filter Car:
          </label>

          <input
            name="filterCarText"
            className="textInput"
            autoComplete="off"
            type="text"
            value={filterCarText}
            onChange={handleChange}
          />

          <select
            name="filterMonth"
            value={monthFilter}
            onChange={handleChange}
          >
            {months.map(m => (
              <option key={m} value={m}>{m}</option>
            ))}
          </select>
        </div>
      </form>

      <div style={{ 'width': '100%' }}>
        <table className='dataTable'>
          <thead>
            <tr className='dataTr'>
              <th className='dataTh'>Nr.</th>
              <th className='dataTh'>Service Description</th>
              <th className='dataTh'>Vehicle</th>
              <th className='dataTh'>Labor Cost</th>
              <th className='dataTh'>Total Cost</th>
              <th className='dataTh'>Completion date</th>
            </tr>
          </thead>
          <tbody>
            {serviceOrdersFiltered != null && serviceOrdersFiltered.map((so, index) => {
              return (
                <React.Fragment key={so.serviceOrderId}>
                  <tr className="dataTr">
                    <td className="dataTh">{index + 1}</td>
                    <td className="dataTh">{so.description}</td>
                    <td className="dataTh">{so.vehicle.brandVehicle} {so.vehicle.modelVehicle}</td>
                    <td className="dataTh">
                      {so.serviceTasks
                        .reduce((sum, task) => sum + parseFloat(task.laborCost), 0)
                        .toFixed(2)}
                    </td>
                    <td className="dataTh">
                      {so.serviceTasks
                        .reduce((sum, task) => sum + parseFloat(task.totalCost), 0)
                        .toFixed(2)}
                    </td>
                    <td className="dataTh">
                      {so.dateFinished
                        ? new Date(so.dateFinished).toLocaleString()
                        : '-'}
                    </td>
                  </tr>

                  {so.serviceTasks.map((st, taskIndex) => (
                    <tr className="dataTr" key={`${so.serviceOrderId}-${taskIndex}`}>
                      <td className="dataTd"></td>
                      <td className="dataTd">{st.name}</td>
                      <td className="dataTd"></td>
                      <td className="dataTd">{st.laborCost}</td>
                      <td className="dataTd">{st.totalCost}</td>
                      <td className="dataTd"></td>
                    </tr>
                  ))}
                </React.Fragment>
              )
            })}
          </tbody>
        </table>
      </div>

    </div>
  )
}

export default RaportPage