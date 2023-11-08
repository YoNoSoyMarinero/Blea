
import { AxiosRequest } from "./utils";
import { AxiosResponseResult } from "../types/globalTypes";
import { RegistrationFormFields } from "../types/globalTypes";
import { LoginFormFields } from "../types/globalTypes";

export const registerUser = async (input: RegistrationFormFields): Promise<AxiosResponseResult> => {
    return await AxiosRequest({ 
        url: "https://localhost:7066/User/register",
        method: "post", 
        input: input 
    });
}

export const loginUser = async (input: LoginFormFields): Promise<AxiosResponseResult> => {
    return await AxiosRequest({ 
        url: "https://localhost:7066/User/login",
        method: "post",
        input: input 
    });
}

export const verifyEmailExists = async (email: string): Promise<AxiosResponseResult> => {
    return await AxiosRequest({ 
        url: `https://localhost:7066/User/check-email/${email}`,
        method: "get" 
    });
}

export const verifyUsernameExists = async (username: string): Promise<AxiosResponseResult> => {
    return await AxiosRequest({ 
        url: `https://localhost:7066/User/check-username/${username}`, 
        method: "get" 
    });
}

export const verifyUser = async (userId: string, token: string): Promise<AxiosResponseResult> => {
    return await AxiosRequest({
        url: `https://localhost:7066/User/confirm/${userId}/${token}`,
        method: "get"
    })
}