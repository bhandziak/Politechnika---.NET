import React, { useState, useContext, useRef } from "react";
import { Link } from 'react-router-dom';


import PopUp from "../../components/PopUp";
import ValidationBox from "../../components/ValidationBox";
import axios from "../../api/axios";
import APIs from "../../api/ApiURL";

const USER_REGEX = /^[a-zA-Z][a-zA-Z0-9-_#]{4,24}$/;
const PASS_REGEX = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,64}$/;

const RegisterPage = () => {
  const popUpRef = useRef();
  
  const [login, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [password2, setPassword2] = useState("");

  const [userRegex, setUserRegex] = useState(false);
  const [passRegex, setPassRegex] = useState(false);
  const [pass2Regex, setPass2Regex] = useState(false);

  const [usernameFocus, setUsernameFocus] = useState(false);
  const [passwordFocus, setPasswordFocus] = useState(false);
  const [password2Focus, setPassword2Focus] = useState(false);


  const handleChange = (e) => {
    popUpRef.current?.hide();
    const { name, value } = e.target;

    if (name === "login") {
      setUsername(value);
      setUserRegex(USER_REGEX.test(value));
    } else if (name === "password") {
      setPassword(value);
      setPassRegex(PASS_REGEX.test(value));
      setPass2Regex(password2 === value);
    } else if (name === "password2") {
      setPassword2(value);
      setPass2Regex(password === value);
    }
  };

  const handleFocusOn = (e) => {
    setUsernameFocus(false);
    setPasswordFocus(false);
    setPassword2Focus(false);

    const { name } = e.target;
    if (name === "login") setUsernameFocus(true);
    else if (name === "password") setPasswordFocus(true);
    else if (name === "password2") setPassword2Focus(true);
  };


  const submitRegister = async (e) => {
    e.preventDefault();

    if (!login || !password || !password2) {
      popUpRef.current?.show("Nazwa użytkownika lub hasło nie może być puste");
      return;
    }

    if (!userRegex || !passRegex) {
      popUpRef.current?.show("Nazwa użytkownika lub hasło nie spełniają kryteriów");
      return;
    }

    if (!pass2Regex) {
      popUpRef.current?.show("Podane hasła są różne!");
      setPassword("");
      setPassword2("");
      return;
    }

    try {
      const data = await axios.post(APIs.REGISTER,
        JSON.stringify({ 
            Login: login,
            Password: password }),
        { headers: { 'Content-Type': 'application/json' } }
      );

      if (data.status === 200) {
        popUpRef.current?.show(data.data.message);
      }
    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }

    setUsername("");
    setPassword("");
    setPassword2("");
  };


  return (
    <div id="mainRegisterLoginPage">
        <h1 className="titleOfPage">
            Register Page
        </h1>
      <form className="loginPanel">
        <label htmlFor="login" className={login ? (userRegex ? "correctValidation" : "wrongValidation") : ""}>
          Username:
        </label>
        <input
          value={login}
          onChange={handleChange}
          onFocus={handleFocusOn}
          name="login"
          id="login"
          autoComplete="off"
          type="text"
          className="textInput"
        /><br />
        <ValidationBox
          regex={userRegex} value={login} focus={usernameFocus}
          text={
            <>
              Has 5 - 24 characters in length<br />
              Has to start with English letter<br />
              Can contain English letter, digits and -_#
            </>
          }
        />

        <label htmlFor="password" className={password ? (passRegex ? "correctValidation" : "wrongValidation") : ""}>
          Password:
        </label>
        <input
          value={password}
          onChange={handleChange}
          onFocus={handleFocusOn}
          name="password"
          id="password"
          type="password"
          className="textInput"
        /><br />
        <ValidationBox
          regex={passRegex} value={password} focus={passwordFocus}
          text={
            <>
              Has 8 - 64 characters in length<br />
              At least one uppercase English letter <br />
              At least one lowercase English letter<br />
              At least one digit and special character
            </>
          }
        />

        <label htmlFor="password2" className={password2 ? (pass2Regex ? "correctValidation" : "wrongValidation") : ""}>
          Repeat password:
        </label>
        <input
          value={password2}
          onChange={handleChange}
          onFocus={handleFocusOn}
          name="password2"
          id="password2"
          type="password"
          className="textInput"
        /><br />
        <ValidationBox
          regex={pass2Regex} value={password2} focus={password2Focus}
          text={<>Passwords have to match<br /></>}
        />

        <PopUp ref={popUpRef} />
        <button className="btn" onClick={submitRegister}>Register</button><br /><br />
        <div>Already have an account? Log in below</div>
        <Link to="/login">Log in</Link>
      </form>
    </div>
  );
};

export default RegisterPage;
