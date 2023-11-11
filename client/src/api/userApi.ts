
import { AxiosRequest } from "./utils";
import { AxiosResponseResult } from "../types/globalTypes";
import { RegistrationFormFields } from "../types/globalTypes";
import { LoginFormFields } from "../types/globalTypes";

export const registerUser = (input: RegistrationFormFields): AxiosResponseResult => {
    return AxiosRequest({ 
        url: "https://localhost:7066/User/register",
        method: "post", 
        input: input 
    });
}

export const loginUser = (input: LoginFormFields): AxiosResponseResult => {
    return AxiosRequest({ 
        url: "https://localhost:7066/User/login",
        method: "post",
        input: input 
    });
}

export const verifyEmailExists = (email: string): AxiosResponseResult => {
    return AxiosRequest({ 
        url: `https://localhost:7066/User/check-email/${email}`,
        method: "get" 
    });
}

export const verifyUsernameExists = (username: string): AxiosResponseResult => {
    return AxiosRequest({ 
        url: `https://localhost:7066/User/check-username/${username}`, 
        method: "get" 
    });
}

export const verifyUser = (userId: string, token: string): AxiosResponseResult => {
    return  AxiosRequest({
        url: `https://localhost:7066/User/confirm/${userId}/${token}`,
        method: "get"
    })
}