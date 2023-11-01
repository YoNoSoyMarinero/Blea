/**
 * Custom hooks performing all the http requests regarding user
 */
import { useFetch } from "./useFetch";

export type UserRegister = {
    firstName: string;
    lastName: string;
    phoneNumber: string;
    userName: string;
    email: string;
    dateOfBirth: string;
    password: string;
    repeatPassword: string;
}

export const useRegisterUser = () => {
    const {commonRequest, isLoading, data, error} = useFetch({url: "https://localhost:7066/User/register", method:"post"});
    const registerUser = (input : UserRegister) => commonRequest({ input });
    return { registerUser, isLoading, data, error};
}