/**
 * This file keeps all the types in the code that are used more than once
 */
import { UseFormRegister, FieldValues } from "react-hook-form";
import { AxiosError } from "axios";

export type ReactHookFormRegisterType = UseFormRegister<FieldValues>;

export type RegistrationFormFields  = {
    firstName: string;
    lastName: string;
    phoneNumber: string;
    userName: string;
    email: string;
    dateOfBirth: string;
    password: string;
    repeatPassword: string;
}

export type LoginFormFields = {
    email: string,
    password: string
}

export type AxiosResponseResult = {
    sendRequest: () => Promise<SendRequestResult>,
    cancelRequest: () => void
};

export type SendRequestResult = {
    data: any;
    error: AxiosError | null;
}
