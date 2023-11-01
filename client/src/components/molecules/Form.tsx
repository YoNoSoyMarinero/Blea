/**
 * This is a reusable form component that will dynamically instance all the react components that are nested in itself from wherever it is called
 * You can optionally pass a react-hook-forms register to pass its props to the input
 */
import { FC, createElement } from "react";
import * as React from "react";
import { ReactNode } from "react";
import { reactHookFormRegisterType } from "../atoms/Input";

export interface IFormProps {
  defaultValues?: any;
  children?: ReactNode;
  buttonLabel?: string;
  onSubmit?: any;
  handleSubmit?: any;
  register?: reactHookFormRegisterType;
  className?: string;
}

const Form: FC<IFormProps> = ({ defaultValues, buttonLabel = "Submit", children, onSubmit, handleSubmit, register, ...rest }) => {
  return (
    <form onSubmit={handleSubmit(onSubmit)} {...rest}>
      <div className="d-flex justify-content-center flex-column">
        {Array.isArray(children)
          ? children.map((child) => {
              return child.props.name
                ? createElement(child.type, { ...{ ...child.props, register, key: child.props.name } })
                : child;
            })
          : children}
      </div>
      <button className="btn btn--brand">{buttonLabel}</button>
    </form>
  );
};

export default Form;
