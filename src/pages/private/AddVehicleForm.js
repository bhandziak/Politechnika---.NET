import React, { useState, useRef, useContext } from "react";
import { useLocation } from 'react-router-dom';
import { AuthContext } from "../../context/AuthProvider";
import PopUp from "../../components/PopUp";
import axios from "../../api/axios";
import APIs from "../../api/ApiURL";
import ValidatedInput from "../../components/ValidatedInput";

const BRAND_REGEX = /^[A-Z][a-zA-Z\s\-]{1,29}$/;
const MODEL_REGEX = /^[A-Za-z0-9\s\-]{1,30}$/;
const VIN_REGEX = /^[A-HJ-NPR-Z0-9]{17}$/;
const REGISTRAL_NUMBER_REGEX = /^[A-Z]{2,3}\s?\d{4,5}[A-Z]{0,2}$/;

const AddVehicleForm = () => {
  const popUpRef = useRef();
  const location = useLocation();
  const detail = location.state;
  const { userId, role } = useContext(AuthContext);

  const [formData, setFormData] = useState({
    brand: "",
    model: "",
    vin: "",
    registralNumber: "",
    year: ""
  });
  const [regexStatus, setRegexStatus] = useState({
    brand: false,
    model: false,
    vin: false,
    registralNumber: false,
    year: false
  })
  const [formFocus, setFormFocus] = useState({
    brand: false,
    model: false,
    vin: false,
    registralNumber: false,
    year: false
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
      brand: name === "brand" ? BRAND_REGEX.test(value) : prev.brand,
      model: name === "model" ? MODEL_REGEX.test(value) : prev.model,
      vin: name === "vin" ? VIN_REGEX.test(value) : prev.vin,
      registralNumber: name === "registralNumber" ? REGISTRAL_NUMBER_REGEX.test(value) : prev.registralNumber,
      year: name === "year" ? (value >= 1850 && value <= 2100) : prev.year
    }));
  };

  const handleFocusOn = (e) => {
    const { name } = e.target;

    setFormFocus({
      brand: name === "brand",
      model: name === "model",
      vin: name === "vin",
      registralNumber: name === "registralNumber",
      year: name === "year"
    });
  };

  const addVehicle = async (e) => {
    e.preventDefault();

    const { brand, model, vin, registralNumber, year } = formData;

    if (!brand || !model || !vin || !registralNumber || !year) {
      popUpRef.current?.show("Wszystkie pola są wymagane.");
      return;
    }
    if (!regexStatus.brand || !regexStatus.model || !regexStatus.vin || !regexStatus.registralNumber || !regexStatus.year) {
      popUpRef.current?.show("Podane dane samochodu nie spełniają kryteriów.");
      return;
    }

    try {
      const response = await axios.post(`${APIs.ADD_VEHICLE}/${detail.customerId}`,
        JSON.stringify({
          BrandVehicle: brand,
          ModelVehicle: model,
          VINVehicle: vin,
          RegistralNumberVehicle: registralNumber,
          YearVehicle: year
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
        setFormData({ brand: "", model: "", vin: "", registralNumber: "", year: "" });
        setRegexStatus({ brand: false, model: false, vin: false, registralNumber: false, year: false });
      }
    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  };

  return (

    <div className='contentColumn'>

      <div id='userInfo2'>
        <div><span className='highlight'>Add vehicle to customer: </span></div>
        <div><span className='highlight'>Name: </span>{detail.nameCustomer}</div>
        <div ><span className='highlight'>Surname: </span>{detail.surnameCustomer}</div>
        <div ><span className='highlight'>Phone number: </span>{detail.phoneNumber}</div>


        <form className="loginPanel">
          <h3>Add Vehicle</h3>

          <ValidatedInput
            htmlName="brand"
            labelText="Brand"
            formData={formData.brand}
            regexStatus={regexStatus.brand}
            formFocus={formFocus.brand}
            type="text"
            handleChange={handleChange}
            handleFocusOn={handleFocusOn}
            validationText={<>Marka musi zaczynać się wielką literą i może zawierać tylko litery, spacje i myślniki.</>}
          />

          <ValidatedInput
            htmlName="model"
            labelText="Model"
            formData={formData.model}
            regexStatus={regexStatus.model}
            formFocus={formFocus.model}
            type="text"
            handleChange={handleChange}
            handleFocusOn={handleFocusOn}
            validationText={<>Model może zawierać litery, cyfry, spacje i myślniki.</>}
          />

          <ValidatedInput
            htmlName="vin"
            labelText="VIN"
            formData={formData.vin}
            regexStatus={regexStatus.vin}
            formFocus={formFocus.vin}
            type="text"
            handleChange={handleChange}
            handleFocusOn={handleFocusOn}
            validationText={<>VIN musi składać się z dokładnie 17 znaków (bez I, O, Q).</>}
          />

          <ValidatedInput
            htmlName="registralNumber"
            labelText="Registral Number"
            formData={formData.registralNumber}
            regexStatus={regexStatus.registralNumber}
            formFocus={formFocus.registralNumber}
            type="text"
            handleChange={handleChange}
            handleFocusOn={handleFocusOn}
            validationText={<>Numer rejestracyjny w formacie np. "KR12345", "PO 1234AB".</>}
          />

          <ValidatedInput
            htmlName="year"
            labelText="Year"
            formData={formData.year}
            regexStatus={regexStatus.year}
            formFocus={formFocus.year}
            type="text"
            handleChange={handleChange}
            handleFocusOn={handleFocusOn}
            validationText={<>Rok musi być z zakresu 1850–2100.</>}
          />

          <PopUp ref={popUpRef} />
          <button className="btn" onClick={addVehicle}>Add Vehicle</button>
        </form>
      </div>
    </div>
  )
}

export default AddVehicleForm
