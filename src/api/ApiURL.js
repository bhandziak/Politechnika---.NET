const APIs = {
//#US1
    LOGIN : "/api/user/login",
    REGISTER : "/api/user/register",
//#US2
    GET_ALL_USERS : "/api/user/getAllUsers",
    SET_ROLE: "/api/user/setRole",
//#US3
    ADD_CUSTOMER: "api/customer/addCustomer",
    GET_ALL_CUSTOMERS: '/api/customer/getCustomers',
//#US4
    CUSTOMER_DETAILS: "api/customer/getDetails",
    ADD_VEHICLE: "api/customer/addVehicle",
//#US5
    SEND_PHOTO: "api/customer/getDetails/addVehicleImage",
//#US6
    GET_ALL_SERVICE_ORDERS: "api/serviceOrder/getAll",
    GET_MECHANICS: "api/user/getMachanics",
    ADD_SERVICE_ORDER: "api/serviceOrder/createOrder"
}

export default APIs;