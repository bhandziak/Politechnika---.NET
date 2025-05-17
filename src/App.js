import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate
} from 'react-router-dom';

import AuthProvider from "./context/AuthProvider";
import RequireAuth from "./context/RequireAuth";
import RedirectIfAuth from "./context/RedirectIfAuth";
import RegisterPage from "./pages/public/RegisterPage";
import LoginPage from "./pages/public/LoginPage";
import CommentPage from "./pages/private/CommentPage";


const App = () => {
  console.log("renderuje app");

  return (
    <AuthProvider>
      <Router>
        <Routes>
          {/* public routes */}
          <Route path="/" element={<Navigate replace to="/login" />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/login" element={
            <RedirectIfAuth>
              <LoginPage />
            </RedirectIfAuth>
          } />

          {/* protected routes */}
          <Route element={<RequireAuth allowedRoles={['user']} />}>
            <Route path="/comment" element={<CommentPage />} />
          </Route>

          {/* 404 */}
          {/* <Route path="*" element={<DefaultPage />} /> */}
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;