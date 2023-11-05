import * as React from "react";
import Form from "../../molecules/Form";
import Input from '../../atoms/Input';
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { registerUser } from "../../../api/userApi";
import { RegistrationFormFields } from "../../../types/globalTypes";
import { UserRegisterSchema } from "./helper";
import { useState } from "react";

const RegisterForm = () => {
  const [formError, setFormError] = useState<string>('');

  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm({ resolver: yupResolver(UserRegisterSchema) });

  const onSubmit = async (userData: RegistrationFormFields): Promise<void> => {
    const response = await registerUser(userData);

    if (response.error) {
      setFormError('Something went wrong, please try again later');
      return;
    }
  
    setFormError('');
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
            formMessage={formError}
          >
            <Input
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
