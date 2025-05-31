import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate
} from 'react-router-dom';

import Layout from "./components/Layout";
import AuthProvider from "./context/AuthProvider";
import RequireAuth from "./context/RequireAuth";
import RedirectIfAuth from "./context/RedirectIfAuth";
import RegisterPage from "./pages/public/RegisterPage";
import LoginPage from "./pages/public/LoginPage";
import CommentPage from "./pages/private/CommentPage";
import SetRolePage from "./pages/private/SetRolePage";
import CustomersPage from "./pages/private/CustomersPage";
import AddCustomerForm from "./pages/private/AddCustomerForm";
import CustomerDetails from "./pages/private/CustomerDetails";
import AddVehicleForm from "./pages/private/AddVehicleForm";
import ServiceOrderPage from "./pages/private/ServiceOrderPage";
import AddServiceOrderForm from "./pages/private/AddServiceOrderForm";
import AddServiceTaskForm from "./pages/private/AddServiceTaskForm";
import ServiceOrderDetails from "./pages/private/ServiceOrderDetails";


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
          <Route path="/" element={<Layout />}>
            {/* All users */}
            <Route element={<RequireAuth allowedRoles={['admin', 'mechanic', 'receptionist', 'user']} />}>
              <Route path="comment" element={<CommentPage />} />
              <Route path="customers" element={<CustomersPage />} />
              <Route path="details/:customerId" element={<CustomerDetails />} />
            </Route>
            {/* Only Admin */}
            <Route element={<RequireAuth allowedRoles={['admin']} />}>
              <Route path="setrole" element={<SetRolePage />} />
            </Route>

            {/* Only Receptionist, Admin */}
            <Route element={<RequireAuth allowedRoles={['receptionist', 'admin']} />}>
              <Route path="addcustomer" element={<AddCustomerForm />} />
              <Route path="addvehicle" element={<AddVehicleForm />} />
              <Route path="addserviceorder" element={<AddServiceOrderForm />} />
            </Route>

            {/* Only Mechanic, Receptionist, Admin */}
            <Route element={<RequireAuth allowedRoles={['mechanic','receptionist', 'admin']} />}>
              <Route path="serviceorder" element={<ServiceOrderPage />} />
              <Route path="serviceorderdetails" element={<ServiceOrderDetails />} />
            </Route>

            {/* Only Mechanic */}
            <Route element={<RequireAuth allowedRoles={['mechanic']} />}>
              <Route path="addservicetask" element={<AddServiceTaskForm />} />
            </Route>
          </Route>

          {/* 404 */}
          {/* <Route path="*" element={<DefaultPage />} /> */}
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;