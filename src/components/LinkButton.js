import React from 'react'
import { Outlet, Link, useLocation } from 'react-router-dom';

const LinkButton = ({ webpath, name, stateObj, cssClass }) => {
  const location = useLocation();
  const currentPath = location.pathname;

  return (
    <Link to={webpath}
      state={stateObj}
      className={currentPath == webpath ? 'navButton navButtonActive ' + cssClass : 'navButton ' + cssClass}
      >
      {name}
    </Link>
  )
}

export default LinkButton