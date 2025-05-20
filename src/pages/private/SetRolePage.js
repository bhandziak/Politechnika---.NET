import React, { useEffect, useState,  useContext, useRef} from 'react';
import axios from '../../api/axios';
import { AuthContext } from '../../context/AuthProvider';
import PopUp from '../../components/PopUp';

const SetRolePage =() => {
    const popUpRef = useRef();
    const { login, userId, role, setAuth } = useContext(AuthContext);
    const [users, setUsers] = useState([]);
    const roles = ['admin', 'mechanic', 'receptionist', 'user'];

    useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await axios.get('/api/user/getAllUsers',{
            headers: {
                'Content-Type': 'application/json',
                'auth': userId
            }
        });
        console.log(response.data.users)
        setUsers(response.data.users);
      } catch (err) {
        popUpRef.current?.show('Błąd pobierania użytkowników: ' + err.message);
      }
    };

    fetchUsers();
  }, []);

  const handleRoleChange = async (selectedUserId, newRole) => {
    console.log(userId);
    try {
      await axios.put(`/api/user/setRole/${selectedUserId}`, 
        { role: newRole },{
            headers: {
                'Content-Type': 'application/json',
                'auth': userId
            }
        } 
    );
      setUsers(prev =>
        prev.map(u => u.userId === selectedUserId ? { ...u, role: newRole } : u)
      );
      popUpRef.current?.show("Sukces");
    } catch (err) {
      popUpRef.current?.show('Błąd zmiany roli: ' + err.message);
    }
  };
  return (
    <div className='content'>
        <PopUp ref={popUpRef} />
      <table className='dataTable'>
        <thead>
          <tr className='dataTr'>
            <th className='dataTh'>Nr.</th>
            <th className='dataTh'>Login</th>
            <th className='dataTh'>Rola</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user,index) => (
            <tr className='dataTr' key={user.userId}>
                <td className='dataTd'>{index + 1}</td>
              <td className='dataTd'>{user.login}</td>
              <td className='dataTd'>
                {user.userId == userId ? 
                user.role :

                    <select
                    value={user.role}
                    onClick={() =>  popUpRef.current?.hide()}
                    onChange={(e) => handleRoleChange(user.userId, e.target.value)}
                    >
                    {roles.map(r => (
                        <option key={r} value={r}>{r}</option>
                    ))}
                    </select>
                }

              </td>
            </tr>
          ))}
        </tbody>
      </table>
      
    </div>
  )
}

export default SetRolePage