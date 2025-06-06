import React, { useEffect, useState, useContext, useRef } from 'react';
import { useLocation } from 'react-router-dom';
import axios from '../../../api/axios';
import APIs from '../../../api/ApiURL';
import { AuthContext } from '../../../context/AuthProvider';
import PopUp from '../../../components/PopUp';
import ValidatedInput from "../../../components/ValidatedInput";

const CommentPage = () => {
  const popUpRef = useRef();
  const location = useLocation();
  const { userId, login, role } = useContext(AuthContext);
  const so = location.state;
  const [comments, setComments] = useState([]);

  const [commentText, setCommentText] = useState("");


  const handleChange = (e) => {
    popUpRef.current?.hide();
    const { name, value } = e.target;

    setCommentText(value);
  };

  const fetchComments = async () => {
    try {
      const ServiceOrderId = so.serviceOrderId;
      const response = await axios.get(`${APIs.GET_COMMENTS}/${ServiceOrderId}`, {
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      });
      if (response.status === 200) {
        console.log(response.data);
        setComments(response.data);
      }

    } catch (err) {
      popUpRef.current?.show(err.response.data || err.message);
    }
  };

  const addComment = async (e) => {
    e.preventDefault();
    try {
      const ServiceOrderId = so.serviceOrderId;
      const response = await axios.post(APIs.ADD_COMMENT,
        JSON.stringify({
          serviceOrderId: ServiceOrderId,
          userId: userId,
          content: commentText
        }),
        {
          headers: {
            'Content-Type': 'application/json'
          },
          withCredentials: true
        });
      if (response.status === 200) {
        console.log(response.data);
        // popUpRef.current?.show(response.data);
        setCommentText("");
        fetchComments();
      }

    } catch (err) {
      popUpRef.current?.show(err.response.data || err.message);
    }
  }


  useEffect(() => {
    fetchComments();
  }, []);
  return (
    <div className='contentColumn'>

      <div id='userInfo2'>
        <div><span className='highlight'>Comments about service order for: </span></div>
        <div><span className='highlight'>Name and Surname: </span>{so.customer.nameCustomer} {so.customer.surnameCustomer}</div>
        <div ><span className='highlight'>Car info: </span>{so.vehicle.brandVehicle} {so.vehicle.modelVehicle} {so.vehicle.registralNumberVehicle}</div>
        <div ><span className='highlight'>Description: </span>{so.description}</div>
        <div ><span className='highlight'>Mechanic: </span>{so.mechanic?.userName}</div>
        <div ><span className='highlight'>Status: </span>{so.statusOrder}</div>
      </div>
      <PopUp ref={popUpRef} />
      <form>
        <br />
        <div id='btnLayout'>
          <ValidatedInput
            htmlName={"Comment"}
            labelText="type comment "
            formData={commentText}
            regexStatus={true}
            formFocus={true}
            type="text"
            handleChange={handleChange}
          />
          <button className="btn" onClick={addComment}>Send</button>
        </div>
      </form>
      <br />
      <br />
      <table className='dataTable'>
        <thead>
          <tr className='dataTr'>
            <th className='dataTh'>Username</th>
            <th className='dataTh'>Role</th>
            <th className='dataTh'>Date</th>
            <th className='dataTh'>Content</th>
          </tr>
        </thead>
        <tbody>
          {comments != null && comments.map((c, index) => (
            <tr className='dataTr' key={c.commentId}>
              <td className='dataTd'>{c.user.userName}</td>
              <td className='dataTd'>{c.user.role}</td>
              <td className='dataTd'>{new Date(c.timestampComment).toLocaleString()}</td>
              <td className='dataTd'>{c.content}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default CommentPage