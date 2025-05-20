import React from 'react'
import { Outlet, Link, useLocation  } from 'react-router-dom';

const LinkButton = ({webpath, name}) => {
  const location = useLocation();
  const currentPath = location.pathname;

  return (
    <Link to={webpath}
    className={currentPath == webpath ? 'navButton navButtonActive' : 'navButton'}>
        {name}
    </Link>
  )
}

export default LinkButton