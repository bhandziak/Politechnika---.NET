import React, { useEffect, useState, useContext, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from '../../api/axios';
import APIs from '../../api/ApiURL';
import { AuthContext } from '../../context/AuthProvider';
import PopUp from '../../components/PopUp';
import ValidatedInput from '../../components/ValidatedInput';

const INT_REGEX = /^(100|[1-9][0-9]?)$/;

const AddUsedPartForm = () => {
    const navigate = useNavigate();
    const popUpRef = useRef();
    const location = useLocation();
    const { userId, role } = useContext(AuthContext);
    const st = location.state;
    const [parts, setParts] = useState([
        {
            partId: 'e3a2f26b-8f63-4c8e-a3a5-1a5c9f68ef01',
            namePart: 'Filtr oleju',
            typePart: 'FO-123',
            unitPrice: 25.50
        },
        {
            partId: '6d8e58ac-c1c3-4a9f-a1c9-3dcb5b0a2a99',
            namePart: 'Klocki hamulcowe',
            typePart: 'KH-200',
            unitPrice: 70.00
        },
        {
            partId: 'b7f07fd3-77ff-4b20-8b7f-e348c3f57ed9',
            namePart: 'Olej silnikowy',
            typePart: '5W30',
            unitPrice: 40.00
        },
        {
            partId: 'f1e7d8d2-9f3c-4e8a-a94a-56c831d6ccf2',
            namePart: 'Akumulator',
            typePart: '60Ah',
            unitPrice: 100.00
        },
        {
            partId: '72a1be87-b44c-49c5-8fc4-727cfda99e6c',
            namePart: 'Filtr powietrza',
            typePart: 'FA-1000',
            unitPrice: 20.00
        }
    ]);

    const [formData, setFormData] = useState({
        partId: "",
        quantity: ""
    });
    const [formFocus, setFormFocus] = useState({
        quantity: false
    })
    const [regexStatus, setRegexStatus] = useState({
        quantity: false
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
            quantity: name === "quantity" ? INT_REGEX.test(value) : prev.quantity
        }));
    };

    const handleFocusOn = (e) => {
        const { name } = e.target;

        setFormFocus({
            quantity: name === "quantity"
        });
    };

    const fetchParts = async () => {
        try {
            const response = await axios.get(APIs.GET_ALL_PARTS, {
                headers: {
                    'Content-Type': 'application/json'
                },
                withCredentials: true
            });
            if (response.status === 200) {
                console.log(response.data);
                if (response.data.length > 0) {
                    setFormData(prev => ({
                        ...prev,
                        partId: response.data[0].partId
                    }));
                    setParts(response.data);
                }
            }

        } catch (err) {
            popUpRef.current?.show(err.response?.data || err.message);
        }
    }

    const assignPart = async (e) => {
        e.preventDefault();

        const { partId, quantity } = formData;

        console.log(formData);

        if (!partId || !quantity) {
            popUpRef.current?.show("Wszystkie pola są wymagane.");
            return;
        }
        if (!regexStatus.quantity) {
            popUpRef.current?.show("Zły format ilości");
            return;
        }

        try {
            const ServiceTaskId = st.serviceTaskId;
            console.log(ServiceTaskId);
            const response = await axios.put(APIs.SET_PART,
                JSON.stringify({
                    ServiceTaskId: ServiceTaskId,
                    PartId: partId,
                    Quantity: quantity
                }),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    withCredentials: true
                }
            );

            if (response.status === 200) {
                popUpRef.current?.show("Suma kosztów Service Task: " + response.data);
                setFormData({ partId: "", quantity: "" });
                setRegexStatus({ quantity: false });
            }
        } catch (err) {
            popUpRef.current?.show(err.response?.data || err.message);
        }
    }

    useEffect(() => {
        fetchParts();
    }, []);
    return (
        <div className='contentColumn'>

            <div id='userInfo2'>
                <div><span className='highlight'>Add part to Service Task: </span></div>
                <div><span className='highlight'>Name: </span>{st.name}</div>
                <div ><span className='highlight'>Labor cost: </span>{st.laborCost}</div>


                <form className="loginPanel">
                    <h3>Assign part</h3>

                    <label htmlFor='part'>
                        Part:
                    </label>
                    <select
                        name="partId"
                        value={formData.partId}
                        onClick={() => popUpRef.current?.hide()}
                        onChange={handleChange}
                    >
                        {parts != null && parts.map(p => (
                            <option key={p.partId} value={p.partId}>{p.namePart} {p.typePart}</option>
                        ))}
                    </select>
                    <br />
                    <ValidatedInput
                        htmlName="quantity"
                        labelText="Quantity"
                        formData={formData.quantity}
                        regexStatus={regexStatus.quantity}
                        formFocus={formFocus.quantity}
                        type="text"
                        handleChange={handleChange}
                        handleFocusOn={handleFocusOn}
                        validationText={<>Liczba całkowita od 1 do 100</>}
                    />

                    <PopUp ref={popUpRef} />
                    <button className="btn" onClick={assignPart}>Assign part</button>
                </form>
            </div>
        </div>
    )
}

export default AddUsedPartForm