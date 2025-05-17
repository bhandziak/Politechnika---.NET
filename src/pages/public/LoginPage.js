import React, { useState, useEffect, useContext } from "react";
import { Link, Navigate, useNavigate } from 'react-router-dom';
import PopUp from "../../components/PopUp";
import axios from "../../api/axios";

import { AuthContext } from "../../context/AuthProvider";
import APIs from "../../api/ApiURL";



const LoginPage = () => {
    const navigate = useNavigate();
    const { login, userId, role, setAuth } = useContext(AuthContext);
    const [formState, setFormState] = useState({
        login: "",
        password: ""
    });

    const [popUpMess, setPopUpMess] = useState("");
    const [stateOfPopUp, setStateOfPopUp] = useState(false);

    const handleChange = (event) => {
        closePopUpMess();

        const { name, value } = event.target;
        setFormState(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const submitLogin = async (event) => {
        event.preventDefault();

        if (formState.login === "" || formState.password === "") {
            showPopUpMess("Nazwa użytkownika lub hasło nie może być puste");
            return;
        }

        try {
            const response = await axios.post(APIs.LOGIN_URL,
                JSON.stringify({
                    Login: formState.login,
                    Password: formState.password
                }),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            );

            const res = response.data;
            

            if (response.status === 200) {

                const userData = res.user;
                console.log("userData: ", userData);

                setAuth(userData.login, userData.id, userData.role);

                sessionStorage.setItem('userInfo', JSON.stringify({
                    login: userData.login,
                    id: userData.id,
                    role: userData.role
                }));

                navigate("/comment");
            }

        } catch (err) {
            let mess = err.response?.data || err.message;
            showPopUpMess(mess);
        }

        setFormState({ login: "", password: "" });
    };

    const showPopUpMess = (mess) => {
        setPopUpMess(mess);
        setStateOfPopUp(true);
    };

    const closePopUpMess = () => {
        setPopUpMess("");
        setStateOfPopUp(false);
    };

    console.log("Current context LOGIN:", login, role);


    return (
        <div id="mainRegisterLoginPage">
            <h1 className="titleOfPage">
                Login Page
            </h1>
            <form className="loginPanel">
                <label htmlFor="login">Login: </label>
                <input
                    value={formState.login}
                    onChange={handleChange}
                    name="login"
                    id="login"
                    autoComplete="off"
                    type="text"
                    className="textInput"
                /><br /><br />
                <label htmlFor="password">Password: </label>
                <input
                    value={formState.password}
                    onChange={handleChange}
                    name="password"
                    id="password"
                    type="password"
                    className="textInput"
                /><br />

                <PopUp state={stateOfPopUp} mess={popUpMess} close={closePopUpMess} />

                <button className="btn" onClick={submitLogin}>Login</button><br /><br />
                <div>Don't have an account? Sign up below</div>
                <Link to="/register">Create an account</Link>
                <Link to="/comment">Comment Page</Link>
            </form>

            <div className="welcomeBlock">
                <div className="logoAndText">
                    <div id="logo" />
                    <div className="bottomText">
                        <div className="highlightText">Lorem ipsum dolor sit amet.</div>
                        consectetur adipiscing elit. Nam egestas arcu quis ex vehicula facilisis...
                    </div>
                </div>
            </div>

            <div id="welcomeBlockSmall">
            </div>
        </div>
    );
};

export default LoginPage;
