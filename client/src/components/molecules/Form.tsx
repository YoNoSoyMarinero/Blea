/**
 * This is a reusable form component that will dynamically instantiate all the React components that are nested in itself from wherever it is called.
 * You can optionally pass a react-hook-forms register to pass its props to the input.
 */
import { FC, createElement, FormHTMLAttributes } from "react";
import * as React from "react";
import { ReactNode } from "react";
import { ReactHookFormRegisterType } from "../../types/globalTypes";

interface IFormProps extends FormHTMLAttributes<HTMLFormElement>{
  children?: ReactNode;
  buttonLabel?: string;
  onSubmit?: any;
  handleSubmit?: any;
  register?: ReactHookFormRegisterType;
  className?: string;
  formMessage?: string;
}

const Form: FC<IFormProps> = ({
  buttonLabel = "Submit",
  children,
  onSubmit,
  handleSubmit,
  register,
  formMessage,
  ...rest 
}) => {
  return (
    <div>
      <p>{formMessage}</p>
      <form onSubmit={handleSubmit(onSubmit)} {...rest}>
        <div className="d-flex justify-content-center flex-column">
          {Array.isArray(children)
            ? children.map((child) => {
                return child.props.name
                  ? createElement(child.type, 
                      { ...{ ...child.props,
                        register,
                          key: child.props.name 
                        } 
                      })
                  : child;
              })
            : children}
        </div>
        <button className="btn btn--brand">{buttonLabel}</button>
      </form>
    </div>
  );
};

export default Form;
