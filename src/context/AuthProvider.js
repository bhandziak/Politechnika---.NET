import React, { createContext, useState, useCallback, useEffect } from 'react';
import axios from '../api/axios';

export const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [login, setUsername] = useState('');
  const [userId, setUserID] = useState(null);
  const [role, setRoles] = useState('');
  const [loading, setLoading] = useState(true);

  const setAuth = useCallback((login, userId, role) => {
    console.log(login, userId, role);
    setUsername(login);
    setUserID(userId);
    setRoles(role);
  }, []);

  useEffect(() => {
    const userInfo = sessionStorage.getItem('userInfo');
    if (userInfo) {
      const userInfoObj = JSON.parse(userInfo);
      if (userInfoObj.id) {
        setAuth(userInfoObj.login, userInfoObj.id, userInfoObj.role);
      }
    }
    setLoading(false);
  }, [setAuth]);

  return (
    <AuthContext.Provider value={{
      login,
      userId,
      role,
      setAuth,
      loading 
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;