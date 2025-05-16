import React, { createContext, useState, useCallback } from 'react';
import axios from '../api/axios';

export const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [username, setUsername] = useState('');
  const [userID, setUserID] = useState(null);
  const [roles, setRoles] = useState([]);

  const setAuth = useCallback((username, userID, roles) => {
    console.log(username, userID, roles);
    setUsername(username);
    setUserID(userID);
    setRoles(roles);
  }, []);

  return (
    <AuthContext.Provider value={{
      username,
      userID,
      roles,
      setAuth
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;