/**
 * Reusable input component that provides all the standard inputHtmlAttributes but also supports react-hook-form.
 */
import { FC, InputHTMLAttributes } from "react";
import * as React from "react";
import { ReactHookFormRegisterType } from "../../types/globalTypes";

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  name: string;
  label?: string;
  error?: string;
  register?: ReactHookFormRegisterType;
  wrapperClass?: string;
  className?: string;
}
  
const Input: FC<InputProps> = ({ 
  register,
  name,
  error,
  label,
  wrapperClass,
  ...rest 
}) => {
  return (
    <div className={wrapperClass}>
      {label && <label htmlFor={name}>{label}</label>}
      <input
        aria-invalid={error ? "true" : "false"}
        {...(register ? register(name) : {})}
        {...rest}
      />
      {error && <span role="alert">{error}</span>}
    </div>
  );
};
  
export default Input;
  