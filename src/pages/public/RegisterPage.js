import React, { useState, useContext } from "react";
import { Link } from 'react-router-dom';
import { AuthContext } from "../../context/AuthProvider";


import PopUp from "../../components/PopUp";
import ValidationBox from "../../components/ValidationBox";
import axios from "../../api/axios";
import APIs from "../../api/ApiURL";

const USER_REGEX = /^[a-zA-Z][a-zA-Z0-9-_#]{4,24}$/;
const PASS_REGEX = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,64}$/;

const RegisterPage = () => {
  const { username: ctxUsername, roles, accessToken } = useContext(AuthContext);

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [password2, setPassword2] = useState("");

  const [userRegex, setUserRegex] = useState(false);
  const [passRegex, setPassRegex] = useState(false);
  const [pass2Regex, setPass2Regex] = useState(false);

  const [usernameFocus, setUsernameFocus] = useState(false);
  const [passwordFocus, setPasswordFocus] = useState(false);
  const [password2Focus, setPassword2Focus] = useState(false);

  const [popUpMess, setPopUpMess] = useState("");
  const [stateOfPopUp, setStateOfPopUp] = useState(false);

  const handleChange = (e) => {
    closePopUpMess();
    const { name, value } = e.target;

    if (name === "username") {
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
    if (name === "username") setUsernameFocus(true);
    else if (name === "password") setPasswordFocus(true);
    else if (name === "password2") setPassword2Focus(true);
  };

  const showPopUpMess = (mess) => {
    setPopUpMess(mess);
    setStateOfPopUp(true);
  };

  const closePopUpMess = () => {
    setPopUpMess('');
    setStateOfPopUp(false);
  };

  const submitRegister = async (e) => {
    e.preventDefault();

    if (!username || !password || !password2) {
      showPopUpMess("Nazwa użytkownika lub hasło nie może być puste");
      return;
    }

    if (!userRegex || !passRegex) {
      showPopUpMess("Nazwa użytkownika lub hasło nie spełniają kryteriów");
      return;
    }

    if (!pass2Regex) {
      showPopUpMess("Podane hasła są różne!");
      setPassword("");
      setPassword2("");
      return;
    }

    try {
      const data = await axios.post(APIs.REGISTER_URL,
        JSON.stringify({ userName: username, password }),
        { headers: { 'Content-Type': 'application/json' } }
      );

      if (data.status === 200) {
        showPopUpMess(data.data.title);
      }
    } catch (err) {
      showPopUpMess(err.response?.data?.message || "Wystąpił błąd");
    }

    setUsername("");
    setPassword("");
    setPassword2("");
  };

  console.log("Current context REGISTER:", ctxUsername, roles, accessToken);

  return (
    <div id="mainRegisterLoginPage">
        <h1 className="titleOfPage">
            Register Page
        </h1>
      <form className="loginPanel">
        <label htmlFor="username" className={username ? (userRegex ? "correctValidation" : "wrongValidation") : ""}>
          Username:
        </label>
        <input
          value={username}
          onChange={handleChange}
          onFocus={handleFocusOn}
          name="username"
          id="username"
          autoComplete="off"
          type="text"
          className="textInput"
        /><br />
        <ValidationBox
          regex={userRegex} value={username} focus={usernameFocus}
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

        <PopUp state={stateOfPopUp} mess={popUpMess} close={closePopUpMess} />

        <button className="btn" onClick={submitRegister}>Register</button><br /><br />
        <div>Already have an account? Log in below</div>
        <Link to="/login">Log in</Link>
      </form>
    </div>
  );
};

export default RegisterPage;
