import React, { useEffect, useState, useRef, useContext } from "react";
import { useLocation } from 'react-router-dom';
import { AuthContext } from "../../../context/AuthProvider";
import PopUp from "../../../components/PopUp";
import axios from "../../../api/axios";
import APIs from "../../../api/ApiURL";
import ValidatedInput from "../../../components/ValidatedInput";

const NAME_REGEX = /^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,29}$/;
const SURNAME_REGEX = /^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż\-]{1,49}$/;
const PHONE_REGEX = /^\+48\d{9}$/;

const AddCustomerForm = () => {
  const { userId } = useContext(AuthContext);
  const popUpRef = useRef();
  const location = useLocation();
  const data = location.state;

  const [formData, setFormData] = useState({
    id: null,
    name: "",
    surname: "",
    phoneNumber: ""
  });
  const [regexStatus, setRegexStatus] = useState({
    name: true,
    surname: true,
    phoneNumber: true
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
          NameCustomer: name,
          SurnameCustomer: surname,
          PhoneNumber: phoneNumber
        }),
        {
          headers: {
            'Content-Type': 'application/json'
          },
          withCredentials: true
        }
      );

      if (response.status === 200) {
        popUpRef.current?.show(response.data.message);
        setFormData({ name: "", surname: "", phoneNumber: "" });
        setRegexStatus({ name: false, surname: false, phoneNumber: false });
      }
    } catch (err) {
      console.log(err);
      popUpRef.current?.show(err.response?.data.title || err.message);
    }
  };


  const updateCustomer = async (e) => {
    e.preventDefault();

    const { id, name, surname, phoneNumber } = formData;

    if (!name || !surname || !phoneNumber) {
      popUpRef.current?.show("Wszystkie pola są wymagane.");
      return;
    }
    if (!regexStatus.name || !regexStatus.surname || !regexStatus.phoneNumber) {
      popUpRef.current?.show("Podane dane klienta nie spełniają kryteriów.");
      return;
    }

    try {
      const response = await axios.put(APIs.UPDATE_CUSTOMER,
        JSON.stringify({
          CustomerId: id,
          NameCustomer: name,
          SurnameCustomer: surname,
          PhoneNumber: phoneNumber
        }),
        {
          headers: {
            'Content-Type': 'application/json'
          },
          withCredentials: true
        }
      );

      if (response.status === 200) {
        popUpRef.current?.show(response.data);
      }
    } catch (err) {
      console.log(err);
      popUpRef.current?.show(err.response?.data.title || err.message);
    }
  };

  useEffect(() => {
    console.log(data);
    if (data?.action == "update") {
      let customer = data.customer;
      setFormData({
        id: customer.customerId,
        name: customer.nameCustomer,
        surname: customer.surnameCustomer,
        phoneNumber: customer.phoneNumber
      })
    }
  }, []);

  return (
    <div className="content">

      <form className="loginPanel">
        {
          data?.action == "update" ?
            <h3>Update Customer</h3>
            :
            <h3>Add Customer</h3>
        }
        <ValidatedInput
          htmlName={"name"}
          labelText="Name"
          formData={formData.name}
          regexStatus={regexStatus.name}
          formFocus={formFocus.name}
          type="text"
          handleChange={handleChange}
          handleFocusOn={handleFocusOn}
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
        {
          data?.action == "update" ?
            <button className="btn" onClick={updateCustomer}>Update Customer</button>
            :
            <button className="btn" onClick={addCustomer}>Add Customer</button>
        }
      </form>
    </div>
  );
};

export default AddCustomerForm;
