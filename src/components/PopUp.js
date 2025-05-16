import React from 'react';

const PopUp = ({ state, mess, close }) => {
  return (
    <>
      {state && (
        <table className="popUp">
          <tbody>
            <tr>
              <th>{mess}</th>
              <th className="closeBtnBox">
                <div className="closeBtn" onClick={close} />
              </th>
            </tr>
          </tbody>
        </table>
      )}
    </>
  );
};

export default PopUp;
