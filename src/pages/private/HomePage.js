import React, { useContext } from 'react'
import { AuthContext } from '../../context/AuthProvider';

const HomePage = () => {
    const { userId, login, role } = useContext(AuthContext);
    return (
        <div className='contentColumn'>
            <h1>Welcome to Car Workshop Panel</h1>
            <div id='userInfo2'>
                <div><span className='highlight'>You are logged as: </span></div>
                <div><span className='highlight'>login: </span>{login}</div>
                <div ><span className='highlight'>role: </span>{role}</div>

                <div><span className='highlight'>As a {role} you can:</span></div>
                <ul>
                    {
                        role == "admin" ?
                            <>
                                <li>See list of users</li>
                                <li>Assign roles to users (Mechanic / Receptionist / Admin)</li>
                                <li>Add new clients</li>
                                <li>Add new vehicle to to a client</li>
                                <li>Create a new service order for a vehicle</li>
                                <li>Change status of service order</li>
                                <li>Display, Add, Update, Delete car parts</li>
                            </>
                            :
                            role == "receptionist" ?
                                <>
                                    <li>Add new clients</li>
                                    <li>Add new vehicle to a client</li>
                                    <li>Create new service order for a vehicle</li>

                                </>
                                :
                                role == "mechanic" ?
                                    <>
                                        <li>Add new service task for a service order</li>
                                        <li>Assign used parts to service task</li>
                                        <li>Change status of service order</li></>
                                    :
                                    <>
                                    </>
                    }
                    <li>Display service orders</li>
                    <li>Add a photo of the vehicle</li>
                    <li>See comments of service order</li>
                    <li>Comment service order</li>
                </ul>
            </div>
        </div>
    )
}

export default HomePage