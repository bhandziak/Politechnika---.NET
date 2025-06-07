import React, { useEffect, useState, useContext, useRef } from 'react';
import axios from '../../../api/axios';
import APIs from '../../../api/ApiURL';
import { AuthContext } from '../../../context/AuthProvider';
import PopUp from '../../../components/PopUp';
import LinkButton from '../../../components/LinkButton';

const DownloadPage = () => {
    const popUpRef = useRef();
    const { login, userId, role, setAuth } = useContext(AuthContext);
    const months = ['select month ...', 'styczeń', 'luty', 'marzec', 'kwiecień', 'maj', 'czerwiec',
        'lipiec', 'sierpień', 'wrzesień', 'październik', 'listopad', 'grudzień'];
    const [monthSelect, setMonthSelect] = useState("");
    const [raport, setRaport] = useState([]);

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
                headers: {
                    'Content-Type': 'application/json'
                },
                withCredentials: true
            });
            if (response.status === 200) {
                console.log(response.data);
                let url = response.data.url;
                window.open(url, '_blank');
            }

        } catch (err) {
            popUpRef.current?.show("Brak raportu na dany miesiąc");
        }
    };



    return (
        <div className='contentColumn'>
            <span>Download raport for chosen month:</span>
            <PopUp ref={popUpRef} />
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

                    <button className="btn additionalMargin" onClick={downloadRaport}>See raport</button>
                </form>
            </div>
        </div>
    )
}

export default DownloadPage