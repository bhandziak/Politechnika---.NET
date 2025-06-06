import React, { useEffect, useState, useRef, useContext } from "react";
import { useLocation } from 'react-router-dom';
import { AuthContext } from "../../../context/AuthProvider";
import PopUp from "../../../components/PopUp";
import axios from "../../../api/axios";
import APIs from "../../../api/ApiURL";
import ValidatedInput from "../../../components/ValidatedInput";

const DECIMAL_REGEX = /^\d+(\,\d{1,2})?$/;

const AddPartForm = () => {
    const { userId } = useContext(AuthContext);
    const popUpRef = useRef();
    const location = useLocation();
    const data = location.state;

    const [formData, setFormData] = useState({
        id: null,
        name: "",
        type: "",
        unitPrice: ""
    });
    const [formFocus, setFormFocus] = useState({
        name: false,
        type: false,
        unitPrice: false
    });
    const [regexStatus, setRegexStatus] = useState({
        unitPrice: true
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
            unitPrice: name === "unitPrice" ? DECIMAL_REGEX.test(value) : prev.unitPrice
        }));
    };

    const handleFocusOn = (e) => {
        const { name } = e.target;

        setFormFocus({
            name: name === "name",
            type: name === "type",
            unitPrice: name === "unitPrice"
        });
    };


    const addPart = async (e) => {
        e.preventDefault();

        const { name, type, unitPrice } = formData;

        if (!name || !type || !unitPrice) {
            popUpRef.current?.show("Wszystkie pola są wymagane.");
            return;
        }

        if (!regexStatus.unitPrice) {
            popUpRef.current?.show("Zły zapis ceny");
            return;
        }


        try {
            const response = await axios.post(APIs.ADD_PART,
                JSON.stringify({
                    namePart: name,
                    typePart: type,
                    unitPrice: unitPrice
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
                setFormData({ name: "", type: "", unitPrice: "" });
            }
        } catch (err) {
            console.log(err);
            popUpRef.current?.show(err.response?.data.title || err.message);
        }
    };

    const updatePart = async (e) => {
        e.preventDefault();

        const { name, type, unitPrice, id } = formData;

        if (!name || !type || !unitPrice) {
            popUpRef.current?.show("Wszystkie pola są wymagane.");
            return;
        }

        if (!regexStatus.unitPrice) {
            popUpRef.current?.show("Zły zapis ceny");
            return;
        }


        try {
            const response = await axios.put(`${APIs.UPDATE_PART}/${id}`,
                JSON.stringify({
                    namePart: name,
                    typePart: type,
                    unitPrice: unitPrice
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
    }

    useEffect(() => {
        console.log(data);
        if (data?.action == "update") {
            let part = data.part;
            setFormData({
                id: part.partId,
                name: part.namePart,
                type: part.typePart,
                unitPrice: part.unitPrice.toString().replace('.', ',')
            })
        }
    }, []);

    return (
        <div className="content">

            <form className="loginPanel">

                <ValidatedInput
                    htmlName={"name"}
                    labelText="Name"
                    formData={formData.name}
                    regexStatus={true}
                    formFocus={formFocus.name}
                    type="text"
                    handleChange={handleChange}
                    handleFocusOn={handleFocusOn}
                />

                <ValidatedInput
                    htmlName="type"
                    labelText="Type"
                    formData={formData.type}
                    regexStatus={true}
                    formFocus={formFocus.type}
                    type="text"
                    handleChange={handleChange}
                    handleFocusOn={handleFocusOn}
                />
                <ValidatedInput
                    htmlName="unitPrice"
                    labelText="Unit Price"
                    formData={formData.unitPrice}
                    regexStatus={regexStatus.unitPrice}
                    formFocus={formFocus.unitPrice}
                    type="text"
                    handleChange={handleChange}
                    handleFocusOn={handleFocusOn}
                    validationText={
                        <>Format : %%,%%</>
                    }
                />


                <PopUp ref={popUpRef} />
                {
                    data?.action == "update" ?
                        <button className="btn" onClick={updatePart}>Update Part</button>
                        :
                        <button className="btn" onClick={addPart}>Add Part</button>
                }

            </form>
        </div>
    )
}

export default AddPartForm