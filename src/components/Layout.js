import React, { useState, useEffect, useContext } from "react";
import { Outlet, Link, useLocation  } from 'react-router-dom';
import { AuthContext } from '../context/AuthProvider';

import LinkButton from "./LinkButton";


const Layout = () => {
  const { login, userId, role, setAuth } = useContext(AuthContext);
  const location = useLocation();
  const currentPath = location.pathname;

  const logOut = () =>{
        setAuth('', null, null);
        sessionStorage.removeItem('userInfo');
  };
  return (
    <>
      <nav id='navigationPanel'>
        <LinkButton webpath='/comment' name='Comment Page' />
        <LinkButton webpath='/customers' name='Customers' />
        { role == 'admin' &&
        <>
            <LinkButton webpath='/setrole' name='Set Role' />
        </>
        }
        { ['receptionist', 'admin'].includes(role)  &&
        <>
            <LinkButton webpath='/serviceorder' name='Service Order' />
        </>
        }

        <div id='rightSide'>
        <div id='userInfo'>
          <div><span className='highlight'>login: </span>{login}</div>
          <div ><span className='highlight'>role: </span>{role}</div>
        </div>
          <div className='navButton' onClick={logOut}>
            Log out
          </div>
          
        </div>
      </nav>

      <main id='layoutContext'>
        <Outlet /> {/* podstrony */}
      </main>
    </>
  );
};

export default Layout;
