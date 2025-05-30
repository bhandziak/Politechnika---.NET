import React, { useState, useRef,useContext  } from "react";
import { AuthContext } from "../../context/AuthProvider";
import PopUp from "../../components/PopUp";
import axios from "../../api/axios";
import APIs from "../../api/ApiURL";
import ValidatedInput from "../../components/ValidatedInput";

const NAME_REGEX = /^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,29}$/;
const SURNAME_REGEX = /^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż\-]{1,49}$/;
const PHONE_REGEX = /^\+48\d{9}$/;

const AddCustomerForm = () => {
  const { userId } = useContext(AuthContext);
  const popUpRef = useRef();

  const [formData, setFormData] = useState({
    name: "",
    surname: "",
    phoneNumber: ""
  });
  const [regexStatus, setRegexStatus] = useState({
    name: false,
    surname: false,
    phoneNumber: false
  })
  const [formFocus, setFormFocus] = useState({
    name: false,
    surname: false,
    phoneNumber: false
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
      name: name === "name" ? NAME_REGEX.test(value) : prev.name,
      surname: name === "surname" ? SURNAME_REGEX.test(value) : prev.surname,
      phoneNumber: name === "phoneNumber" ? PHONE_REGEX.test(value) : prev.phoneNumber
    }));
  };

  const handleFocusOn = (e) => {
    const { name } = e.target;

    setFormFocus({
      name: name === "name",
      surname: name === "surname",
      phoneNumber: name === "phoneNumber"
    });
  };

  const addCustomer = async (e) => {
    e.preventDefault();

    const { name, surname, phoneNumber } = formData;

    if (!name || !surname || !phoneNumber) {
      popUpRef.current?.show("Wszystkie pola są wymagane.");
      return;
    }
    if (!regexStatus.name || !regexStatus.surname || !regexStatus.phoneNumber) {
      popUpRef.current?.show("Podane dane klienta nie spełniają kryteriów.");
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
          'Content-Type': 'application/json'
       },
       withCredentials : true
      }
      );

      if (response.status === 200) {
        popUpRef.current?.show(response.data.message);
        setFormData({ name: "", surname: "", phoneNumber: "" });
        setRegexStatus({ name: false, surname: false, phoneNumber: false });
      }
    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  };

  return (
    <div className="content">
      
      <form className="loginPanel">
        <h3>Add Customer</h3>
        
          <ValidatedInput 
            htmlName={"name"}
            labelText="Name"
            formData={formData.name}
            regexStatus = {regexStatus.name}
            formFocus = {formFocus.name}
            type="text"
            handleChange = {handleChange}
            handleFocusOn = {handleFocusOn}
            validationText={<>Imię musi zaczynać się wielką literą i zawierać tylko litery.</>}
            />

        <ValidatedInput 
          htmlName="surname"
          labelText="Surname"
          formData={formData.surname}
          regexStatus={regexStatus.surname}
          formFocus={formFocus.surname}
          type="text"
          handleChange={handleChange}
          handleFocusOn={handleFocusOn}
          validationText={
            <>Nazwisko musi zaczynać się wielką literą i zawierać tylko litery lub myślnik.</>
          }
        />
        <ValidatedInput 
          htmlName="phoneNumber"
          labelText="Phone Number"
          formData={formData.phoneNumber}
          regexStatus={regexStatus.phoneNumber}
          formFocus={formFocus.phoneNumber}
          type="text"
          handleChange={handleChange}
          handleFocusOn={handleFocusOn}
          validationText={
            <>Numer telefonu musi być w formacie +48123456789.</>
          }
        />


        <PopUp ref={popUpRef} />
        <button className="btn" onClick={addCustomer}>Add Customer</button>
      </form>
    </div>
  );
};

export default AddCustomerForm;
