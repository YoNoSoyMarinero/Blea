import * as React from "react";
import { useEffect, useState } from "react";
import { useParams } from 'react-router-dom'; // Import useParams
import { verifyUser } from "../../api/userApi";

const UserVerify = () => {
//    const [isVerified, setIsVerified] = useState<boolean | null>(null);
//    const [error, setError] = useState<string | null>(null); // Add this line
//    const { userId, token } = useParams<{ userId: string, token: string }>(); // Get params from URL

//    useEffect(() => {
//        const verify = async () => {
//            const response = await verifyUser(userId, token);
//            if (response.error) {
//                setError('Error 404: Link expired'); // Set error state if link expired
//            } else {
//                setIsVerified(true); // Set isVerified state to true if success response
//            }
//        };
//        verify();
//    }, [userId, token]);

   return (
       <div>
           {/* {isVerified ? 'Success' : error} */}
       </div>
   );
};

export default UserVerify;
