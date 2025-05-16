import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate
} from 'react-router-dom';

import AuthProvider from "./context/AuthProvider";
import RegisterPage from "./pages/public/RegisterPage";
import LoginPage from "./pages/public/LoginPage";


const App = () => {
  console.log("renderuje app");

  return (
    <AuthProvider>
      <Router>
        <Routes>
          {/* public routes */}
          <Route path="/" element={<Navigate replace to="/login" />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/login" element={<LoginPage />} />

          {/* protected routes */}
          {/* <Route element={<RequireAuth allowedRoles={['user']} />}>
            <Route path="/message" element={<MessagePage />} />
            <Route path="/editprofile" element={<EditProfile />} />
          </Route> */}

          {/* 404 */}
          {/* <Route path="*" element={<DefaultPage />} /> */}
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;