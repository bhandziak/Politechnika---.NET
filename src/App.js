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
import CommentPage from "./pages/private/serviceOrder/CommentPage";
import SetRolePage from "./pages/private/admin/SetRolePage";
import CustomersPage from "./pages/private/customer/CustomersPage";
import AddCustomerForm from "./pages/private/customer/AddCustomerForm";
import CustomerDetails from "./pages/private/customer/CustomerDetails";
import AddVehicleForm from "./pages/private/customer/AddVehicleForm";
import ServiceOrderPage from "./pages/private/serviceOrder/ServiceOrderPage";
import AddServiceOrderForm from "./pages/private/serviceOrder/AddServiceOrderForm";
import AddServiceTaskForm from "./pages/private/mechanic/AddServiceTaskForm";
import ServiceOrderDetails from "./pages/private/serviceOrder/ServiceOrderDetails";
import AddUsedPartForm from "./pages/private/mechanic/AddUsedPartForm";
import HomePage from "./pages/private/HomePage";
import PartsPage from "./pages/private/part/PartsPage";
import AddPartForm from "./pages/private/part/AddPartForm";
import RaportPage from "./pages/private/raport/raportPage";
import DownloadPage from "./pages/private/raport/DownloadPage";


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
              <Route path="home" element={<HomePage />} />
              <Route path="customers" element={<CustomersPage />} />
              <Route path="details/:customerId" element={<CustomerDetails />} />
              <Route path="serviceorder" element={<ServiceOrderPage />} />
            </Route>
            {/* Only Admin */}
            <Route element={<RequireAuth allowedRoles={['admin']} />}>
              <Route path="setrole" element={<SetRolePage />} />
              <Route path="addpart" element={<AddPartForm />} />
              <Route path="downloadraport" element={<DownloadPage />} />
            </Route>

            {/* Only Receptionist, Admin */}
            <Route element={<RequireAuth allowedRoles={['receptionist', 'admin']} />}>
              <Route path="addcustomer" element={<AddCustomerForm />} />
              <Route path="addvehicle" element={<AddVehicleForm />} />
              <Route path="addserviceorder" element={<AddServiceOrderForm />} />
              <Route path="raport" element={<RaportPage />} />
            </Route>

            {/* Only Mechanic, Receptionist, Admin */}
            <Route element={<RequireAuth allowedRoles={['mechanic', 'receptionist', 'admin']} />}>
              <Route path="serviceorderdetails" element={<ServiceOrderDetails />} />
              <Route path="parts" element={<PartsPage />} />
            </Route>

            {/* Only Mechanic */}
            <Route element={<RequireAuth allowedRoles={['mechanic']} />}>
              <Route path="addservicetask" element={<AddServiceTaskForm />} />
              <Route path="addusedpart" element={<AddUsedPartForm />} />
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