import React, { useState, useRef,useContext  } from "react";
import { AuthContext } from "../../context/AuthProvider";
import PopUp from "../../components/PopUp";
import axios from "../../api/axios";
import APIs from "../../api/ApiURL";

const AddCustomerForm = () => {
  const { userId } = useContext(AuthContext);
  const popUpRef = useRef();

  const [formData, setFormData] = useState({
    name: "",
    surname: "",
    phoneNumber: ""
  });

  const handleChange = (e) => {
    popUpRef.current?.hide();
    const { name, value } = e.target;

    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const addCustomer = async (e) => {
    e.preventDefault();

    const { name, surname, phoneNumber } = formData;

    if (!name || !surname || !phoneNumber) {
      popUpRef.current?.show("Wszystkie pola są wymagane.");
      return;
    }

    try {
      const response = await axios.post(APIs.ADD_CUSTOMER,
        JSON.stringify({ 
          FirstName: name,
          LastName: surname,
          PhoneNumber: phoneNumber
         }),
        { headers: { 
          'Content-Type': 'application/json',
          'auth': userId
       } }
      );

      if (response.status === 200) {
        popUpRef.current?.show(response.data.message);
        setFormData({ name: "", surname: "", phoneNumber: "" });
      }
    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  };

  return (
    <div className="content">
      
      <form className="loginPanel">
        <h3>Add Customer</h3>
        <label htmlFor="name">Name:</label>
        <input
          name="name"
          id="name"
          className="textInput"
          autoComplete="off"
          value={formData.name}
          onChange={handleChange}
        /><br />

        <label htmlFor="surname">Surname:</label>
        <input
          name="surname"
          id="surname"
          className="textInput"
          autoComplete="off"
          value={formData.surname}
          onChange={handleChange}
        /><br />

        <label htmlFor="phoneNumber">Phone Number:</label>
        <input
          name="phoneNumber"
          id="phoneNumber"
          className="textInput"
          autoComplete="off"
          value={formData.phoneNumber}
          onChange={handleChange}
        /><br />

        <PopUp ref={popUpRef} />
        <button className="btn" onClick={addCustomer}>Add Customer</button>
      </form>
    </div>
  );
};

export default AddCustomerForm;
