/**
 * This component utilizes reusable Form and Input components to create a registration form
 * Here our job is to define react-hook-form schema, yup schema onSubmit method and instance actual inputs inside the form
 */ 
import * as React from "react";
import Form from "../../molecules/Form";
import Input from '../../atoms/Input';
import './RegisterForm.css';
import { useForm } from "react-hook-form";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useRegisterUser } from "../../../hooks/api/userApi";


// Schema for react-hook-form inputs
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

const nameRegex = /^[A-Za-z ]*$/;
const phoneRegex = /^(\+?\d{0,4})?\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{4}\)?)?$/;

// Schema for yup validation
const UserRegisterSchema = yup.object().shape({
  firstName: yup.string().matches(nameRegex, "Please enter valid first name").max(20).required(),
  lastName: yup.string().matches(nameRegex, "Please enter valid last name").max(20).required(),
  phoneNumber: yup.string().matches(phoneRegex, "Please enter a valid phone number").required(),
  userName: yup.string().required().max(15),
  email: yup.string().email("Please type in a valid email").required("This field is required"),
  dateOfBirth: yup.date().nullable().transform((curr, orig) => orig === "" ? null : curr).required("This field is required"),
  password: yup.string().max(32, "Max password length is 32").required("This field is required"),
  repeatPassword: yup.string().required("This field is required").oneOf([yup.ref("password")], "Your passwords do not match.")
});

const RegisterForm = () => {
  const {register, handleSubmit, formState: { errors }} = useForm({ resolver: yupResolver(UserRegisterSchema) });
  const {registerUser, isLoading, data, error } = useRegisterUser();

  // Perform the api call for registration on the data provided as parameter, this function will only be called after yup validation is fullfilled
  const onSubmit = async (userData: RegistrationFormFields) => {
    console.log(userData);

    await registerUser(userData);

    console.log(data);
    console.log(error);
  };

  return (
    <div className="container">
      <div className="row">
        <div className="col-sm-10 col-lg-6">
          <Form
            buttonLabel="Register"
            register={register}
            handleSubmit={handleSubmit}
            onSubmit={onSubmit}
            className="change-form"
          >
            <Input
              placeholder='test'
              name='firstName'
              label="First Name"
              type="text"
              error={errors.firstName?.message}
            />
            <Input
              type="text"
              label="Last Name"
              name="lastName"
              error={errors.lastName?.message}
            />
            <Input
              type="text"
              label="Phone number"
              name="phoneNumber"
              error={errors.phoneNumber?.message}
            />
            <Input
              type="text"
              label="Username"
              name="userName"
              error={errors.userName?.message}
            />
            <Input
              type="text"
              label="Email"
              name="email"
              error={errors.email?.message}
            />
            <Input
              type="date"
              label="Date of birth"
              name="dateOfBirth"
              error={errors.dateOfBirth?.message}
            />
            <Input
              type="text"
              label="Password"
              name="password"
              error={errors.password?.message}
            />
            <Input
              type="text"
              label="Repeat passowrd"
              name="repeatPassword"
              error={errors.repeatPassword?.message}
            />
          </Form>
        </div>
      </div>
    </div>
    
  );
};

export default RegisterForm;
