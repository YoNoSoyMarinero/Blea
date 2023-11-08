import * as React from "react";
import Form from "../../molecules/Form";
import Input from '../../atoms/Input';
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { LoginFormFields } from "../../../types/globalTypes";
import { useForm } from "react-hook-form";
import { loginUser } from "../../../api/userApi";
import { useState } from "react";

const userLoginSchema = yup.object().shape({
    email: yup.string(),
    password: yup.string()
})

const LoginForm = () => {
  const [formError ,setFormError] = useState<string>('');

  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm({ resolver: yupResolver(userLoginSchema) });

  const onSubmit = async (userData: LoginFormFields): Promise<void> => {
    const response = await loginUser(userData);

    if (response.error && response.error.response) {
        const status = response.error.response.status;
        setFormError(status >= 400 && status < 500 ? 'Invalid email or password' : 'Something went wrong, please try again later');
    } else if (response.data) {
        setFormError('');
    } else {
        setFormError('An unknown error occurred.');
    }
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
              type="text"
              label="Email"
              name="email"
              error={errors.email?.message}
            />
            <Input
              type="text"
              label="Password"
              name="password"
              error={errors.password?.message}
            />
          </Form>
        </div>
      </div>
    </div>
  );
};

export default LoginForm;
