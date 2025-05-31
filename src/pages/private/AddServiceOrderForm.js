import React, { useEffect, useState, useContext, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from '../../api/axios';
import { AuthContext } from '../../context/AuthProvider';
import APIs from '../../api/ApiURL';
import PopUp from '../../components/PopUp';
import ValidatedInput from '../../components/ValidatedInput';


const AddServiceOrderForm = () => {
    const navigate = useNavigate();
    const popUpRef = useRef();
    const location = useLocation();
    const serviceOrder = location.state;
    const { userId, role } = useContext(AuthContext);
    const [mechanics, setMechanics] = useState([]);


    const [formData, setFormData] = useState({
        description: "",
        mechanicId: null
    });
    const [formFocus, setFormFocus] = useState({
        description: false
    })

    const handleChange = (e) => {
        popUpRef.current?.hide();
        const { name, value } = e.target;

        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleFocusOn = (e) => {
        const { name } = e.target;

        setFormFocus({
            description: name === "description"
        });
    };

    const fetchMechanics = async () => {
        try {
            const response = await axios.get(APIs.GET_MECHANICS, {
                headers: {
                    'Content-Type': 'application/json'
                },
                withCredentials: true
            });
            if (response.status === 200) {
                console.log(response.data)
                if (response.data.mechanics.length > 0) {
                    setFormData(prev => ({
                        ...prev,
                        mechanicId: response.data.mechanics[0].mechanicId
                    }));
                    setMechanics(response.data.mechanics);
                }
            }

        } catch (err) {
            popUpRef.current?.show(err.response.data || err.message);
        }
    };

    useEffect(() => {
        fetchMechanics();
    }, []);

    const createServiceOrder = async (e) => {
        e.preventDefault();

        const { description, mechanicId } = formData;

        console.log(formData);

        if (!description || !mechanicId) {
            popUpRef.current?.show("Wszystkie pola są wymagane.");
            return;
        }

        try {
            const response = await axios.put(APIs.ADD_SERVICE_ORDER,
                JSON.stringify({
                    ServiceOrderId: serviceOrder.serviceOrderId,
                    Description: description,
                    UserId: mechanicId
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
                navigate("/serviceorder");
            }
        } catch (err) {
            popUpRef.current?.show(err.response?.data || err.message);
        }
    };

    return (

        <div className='contentColumn'>

            <div id='userInfo2'>
                <div><span className='highlight'>Add service order for </span></div>
                <div><span className='highlight'>Name: </span>{serviceOrder.customer.nameCustomer}</div>
                <div ><span className='highlight'>Surname: </span>{serviceOrder.customer.surnameCustomer}</div>
                <div ><span className='highlight'>Car: </span>{serviceOrder.vehicle.brandVehicle} {serviceOrder.vehicle.modelVehicle}, {serviceOrder.vehicle.registralNumberVehicle}</div>

                <form className="loginPanel">
                    <h3>Add Service Order</h3>
                    <ValidatedInput
                        htmlName="description"
                        labelText="Description"
                        formData={formData.description}
                        regexStatus={true}
                        formFocus={formFocus.description}
                        type="text"
                        handleChange={handleChange}
                        handleFocusOn={handleFocusOn}
                        validationText={<></>}
                    />

                    <label htmlFor='mechanic'>
                        Mechanic:
                    </label>
                    <select
                        name="mechanicId"
                        value={formData.mechanicId}
                        onClick={() => popUpRef.current?.hide()}
                        onChange={handleChange}
                    >
                        {mechanics != null && mechanics.map(m => (
                            <option key={m.mechanicId} value={m.mechanicId}>{m.userName}</option>
                        ))}
                    </select>
                    <br />
                    <PopUp ref={popUpRef} />
                    <button className="btn" onClick={createServiceOrder}>Create</button>
                </form>

            </div>
        </div>
    )
}

export default AddServiceOrderForm