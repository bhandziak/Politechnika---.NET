import React, { useState, useEffect, useContext } from "react";
import { Outlet, Link, useLocation  } from 'react-router-dom';
import { AuthContext } from '../context/AuthProvider';


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
        <Link to="/comment"
         className={currentPath == '/comment' ? 'navButton navButtonActive' : 'navButton'} >
        Comment Page</Link>
        { role == 'admin' &&
            <Link to="/setrole"
            className={currentPath == '/setrole' ? 'navButton navButtonActive' : 'navButton'}>
            Set Role</Link>
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
        <Outlet /> {/* tu będą renderowane podstrony */}
      </main>
    </>
  );
};

export default Layout;
