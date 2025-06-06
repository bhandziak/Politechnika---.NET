import React, { useEffect, useState, useContext, useRef } from 'react';
import axios from '../../../api/axios';
import APIs from '../../../api/ApiURL';
import { AuthContext } from '../../../context/AuthProvider';
import PopUp from '../../../components/PopUp';
import LinkButton from '../../../components/LinkButton';

const PartsPage = () => {
  const popUpRef = useRef();
  const { login, userId, role } = useContext(AuthContext);
  const [parts, setParts] = useState([]);

  const fetchParts = async () => {
    try {
      const response = await axios.get(APIs.GET_ALL_PARTS, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log(response.data);
        setParts(response.data);
      }

    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  }

  const deletePart = async (partId) => {
    try {
      const response = await axios.delete(`${APIs.DELETE_PART}/${partId}`, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log(response.data);
        popUpRef.current?.show(response.data);
        fetchParts();
      }

    } catch (err) {
      popUpRef.current?.show(err.response?.data || err.message);
    }
  }

  useEffect(() => {
    fetchParts();
  }, []);

  return (
    <div className='contentColumn'>
      <div id='userInfo2'>
        {['receptionist', 'admin'].includes(role) &&
          <LinkButton webpath='/addpart' name='Add part' />
        }
      </div>


      <PopUp ref={popUpRef} />
      <div style={{ 'width': '100%' }}>
        <table className='dataTable'>
          <thead>
            <tr className='dataTr'>
              <th className='dataTh'>Nr.</th>
              <th className='dataTh'>Name</th>
              <th className='dataTh'>Type</th>
              <th className='dataTh'>Unit price</th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {parts != null && parts.map((p, index) => (
              <tr className='dataTr' key={p.partId}>
                <td className='dataTd'>{index + 1}</td>
                <td className='dataTd'>{p.namePart}</td>
                <td className='dataTd'>{p.typePart}</td>
                <td className='dataTd'>{p.unitPrice}</td>

                {
                  role == "admin" &&
                  <>
                  <td>
                    <LinkButton webpath='/addpart' name='Update' stateObj={
                      { action: "update", part: p }
                    }
                    cssClass={'updateButton'} />
                  </td>
                  <td>
                    <button className="btn deleteButton" onClick={() => deletePart(p.partId)}>Delete</button>
                  </td>
                  </>
                }
              </tr>
            ))}
          </tbody>
        </table>
      </div>


    </div>
  )
}

export default PartsPage