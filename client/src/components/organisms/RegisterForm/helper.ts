import * as yup from "yup";
import { verifyEmailExists, verifyUsernameExists } from "../../../api/userApi";
import { AxiosResponseResult, SendRequestResult } from "../../../types/globalTypes";
import { debounce } from "../../../utils/debounce";

const nameRegex = /^[A-Za-z]+$/;
const phoneRegex = /^(\+?\d{0,4})?\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{4}\)?)?$/;
const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]+$/;

const verifyEmailYup = debounce(async (email: string): Promise<boolean> => {
  if (!email) return true;
  const { sendRequest, cancelRequest }: AxiosResponseResult = verifyEmailExists(email);
  const response: SendRequestResult = await sendRequest();
  if (response.error) return true;
  return !response.data?.emailFound;
}, 500);

const verifyUsernameYup = debounce(async (userName: string): Promise<boolean> => {
  if (!userName) return true;
  const { sendRequest, cancelRequest }: AxiosResponseResult = verifyUsernameExists(userName);
  const response: SendRequestResult = await sendRequest();
  if (response.error) return true;
  return !response.data?.usernameFound;
}, 500);

export const UserRegisterSchema = yup.object().shape({
  firstName: yup
    .string()
    .matches(nameRegex, "Please enter valid first name")
    .max(20)
    .required("This field is required"),
  lastName: yup
    .string()
    .matches(nameRegex, "Please enter valid last name")
    .max(20)
    .required("This field is required"),
  phoneNumber: yup
    .string()
    .matches(phoneRegex, "Please enter a valid phone number")
    .required("This field is required"),
  userName: yup
    .string()
    .required("This field is required")
    .max(15)
    .min(6, "Username must be atleast 6 characters long")
    .test("username-exists", "Username already exists",  async (value) => {
      return verifyUsernameYup(value);
    }),
  email: yup
    .string()
    .email("Please type in a valid email")
    .required("This field is required")
    .test("email-exists", "Email already exists", async (value) => {
      const found = await verifyEmailYup(value);
      console.log(found, 'ovde');
      return found;
    }),
  dateOfBirth: yup
    .date()
    .nullable()
    .transform((curr, orig) => orig === "" ? null : curr)
    .required("This field is required"),
  password: yup
    .string()
    .max(32, "Max password length is 32")
    .matches(
        passwordRegex,
        "Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character (!@#$%^&*)"
    )
    .required("This field is required"),
  repeatPassword: yup
    .string()
    .required("This field is required")
    .oneOf([yup.ref("password")], "Your passwords do not match.")
});
