import React, { useEffect, useState, useContext, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from '../../api/axios';
import { AuthContext } from '../../context/AuthProvider';
import APIs from '../../api/ApiURL';
import PopUp from '../../components/PopUp';
import ValidatedInput from '../../components/ValidatedInput';

const DECIMAL_REGEX = /^\d+(\,\d{1,2})?$/;

const AddServiceTaskForm = () => {
  const navigate = useNavigate();
  const popUpRef = useRef();
  const location = useLocation();
  const so = location.state;
  const { userId, role } = useContext(AuthContext);

  const [formData, setFormData] = useState({
    name: "",
    laborCost: ""
  });
  const [formFocus, setFormFocus] = useState({
    name: false,
    laborCost: false
  });

  const [regexStatus, setRegexStatus] = useState({
    laborCost: false
  })

  const handleChange = (e) => {
    popUpRef.current?.hide();
    const { name, value } = e.target;

    setFormData(prev => ({
      ...prev,
      [name]: value
    }));

    setRegexStatus(prev => ({
      ...prev,
      laborCost: name === "laborCost" ? DECIMAL_REGEX.test(value) : prev.laborCost
    }));
  };
  const handleFocusOn = (e) => {
    const { name } = e.target;

    setFormFocus({
      name: name === "name",
      laborCost: name === "laborCost"
    });
  };

  const createServiceTask = async (e) => {
    e.preventDefault();

    const { name, laborCost } = formData;

    console.log(formData);

    if (!name || !laborCost) {
      popUpRef.current?.show("Wszystkie pola są wymagane.");
      return;
    }
    if (!regexStatus.laborCost) {
      popUpRef.current?.show("Zły zapis kosztów");
      return;
    }

    try {
      const ServiceOrderId = so.serviceOrderId;
      const response = await axios.post(APIs.ADD_SERVICE_TASK,
        JSON.stringify({
          serviceOrderId: ServiceOrderId,
          name: name,
          laborCost: laborCost
        }),
        {
          headers: {
            'Content-Type': 'application/json'
          },
          withCredentials: true
        }
      );

      if (response.status === 200) {
        console.log(response.data);
        popUpRef.current?.show(response.data);
        setFormData({ name: "", laborCost: "" });
        setRegexStatus({ laborCost: false });
      }
    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  };

  return (
    <div className='contentColumn'>

      <div id='userInfo2'>
        <div><span className='highlight'>Add service task for </span></div>
        <div><span className='highlight'>Name and Surname: </span>{so.customer.nameCustomer} {so.customer.surnameCustomer}</div>
        <div ><span className='highlight'>Car info: </span>{so.vehicle.brandVehicle} {so.vehicle.modelVehicle} {so.vehicle.registralNumberVehicle}</div>
        <div ><span className='highlight'>Description: </span>{so.description}</div>
        <div ><span className='highlight'>Mechanic: </span>{so.mechanic?.userName}</div>
        <div ><span className='highlight'>Status: </span>{so.statusOrder}</div>

        <form className="loginPanel">
          <h3>Add Service Task</h3>

          <ValidatedInput
            htmlName="name"
            labelText="Name"
            formData={formData.name}
            regexStatus={true}
            formFocus={formFocus.name}
            type="text"
            handleChange={handleChange}
            handleFocusOn={handleFocusOn}
          />

          <ValidatedInput
            htmlName="laborCost"
            labelText="Labor Cost"
            formData={formData.laborCost}
            regexStatus={regexStatus.laborCost}
            formFocus={formFocus.laborCost}
            type="text"
            handleChange={handleChange}
            handleFocusOn={handleFocusOn}
            validationText={<>Format kosztu : %%,%%</>}
          />

          <PopUp ref={popUpRef} />
          <button className="btn" onClick={createServiceTask}>Create Service Task</button>
        </form>
      </div>
    </div>
  )
}

export default AddServiceTaskForm