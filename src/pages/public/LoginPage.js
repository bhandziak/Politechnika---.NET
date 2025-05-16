import React, { useState, useEffect, useContext } from "react";
import { Link, Navigate, useNavigate } from 'react-router-dom';
import PopUp from "../../components/PopUp";
import axios from "../../api/axios";

import { AuthContext } from "../../context/AuthProvider";
import APIs from "../../api/ApiURL";

const LoginPage = () => {
    const { username, userID, roles } = useContext(AuthContext);
    const [formState, setFormState] = useState({
        username: "",
        password: ""
    });

    const [popUpMess, setPopUpMess] = useState("");
    const [stateOfPopUp, setStateOfPopUp] = useState(false);
    const [directory, setDirectory] = useState("");

    useEffect(() => {
        // redirect to ... if user is logged in
        if (!userID) {
            const userIDFromCookie = sessionStorage.getItem('userID');
            if (userIDFromCookie) {
                setDirectory("/");
            }
        } else {
            setDirectory("/");
        }
    }, [userID]);

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

        if (formState.username === "" || formState.password === "") {
            showPopUpMess("Nazwa użytkownika lub hasło nie może być puste");
            return;
        }

        try {
            const response = await axios.post(APIs.LOGIN_URL,
                JSON.stringify({
                    userName: formState.username,
                    password: formState.password
                }),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            );

            const res = response.data.resultData;

            if (response.status === 200) {

                const userData = res.user;

                //setAuth(userData.userName, userData.id, ["user"]);

                const userInfo = {
                    name: userData.userName,
                    userID: userData.id,
                    roles: roles
                };
                sessionStorage.setItem('userInfo', JSON.stringify(userInfo));

                setDirectory("/");
            }

        } catch (err) {
            let mess = err.response?.data?.title || err.message;
            showPopUpMess(mess);
        }

        setFormState({ username: "", password: "" });
    };

    const showPopUpMess = (mess) => {
        setPopUpMess(mess);
        setStateOfPopUp(true);
    };

    const closePopUpMess = () => {
        setPopUpMess("");
        setStateOfPopUp(false);
    };

    console.log("Current context LOGIN:", username, roles);

    if (directory) {
        return <Navigate to={directory} />;
    }

    return (
        <div id="mainRegisterLoginPage">
            <h1 className="titleOfPage">
                Login Page
            </h1>
            <form className="loginPanel">

                <label htmlFor="username">Username: </label>
                <input
                    value={formState.username}
                    onChange={handleChange}
                    name="username"
                    id="username"
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
                <Link to="/">Comment Page</Link>
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
