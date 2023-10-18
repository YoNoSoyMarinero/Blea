import React, {useState} from "react"
import './RegistrationPageStyle.css'
import video from "../../assets/football.mp4"


export const RegistartionPage = (props) => {

  const [firstname, setFirstname ] = useState("");
  const [lastname, setLastname ] = useState("");
  const [phoneNumber, setPhoneNumber ] = useState("");
  const [username, setUsername ] = useState("");
  const [email, setEmail] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");
  const [password, setPassword ] = useState("");
  const [passwordRepeat, setPasswordRepeat ] = useState("");
  const [error, setError ] = useState(false);

  const [firstnameErrorMessage, setFirstnameErrorMessage ] = useState("");
  const [lastnameErrorMessage, setLastnameErrorMessage ] = useState("");
  const [phoneNumberErrorMessage, setPhoneNumberErrorMessage ] = useState("");
  const [usernameErrorMessage, setUsernameErrorMessage ] = useState("");
  const [emailErrorMessage, setEmailErrorMessage] = useState("");
  const [dateOfBirthErrorMessage, setDateOfBirthErrorMessage] = useState("");
  const [passwordErrorMessage, setPasswordErrorMessage ] = useState("");
  const [passwordRepeatErrorMessage, setPasswordRepeatErrorMessage ] = useState("");

  function isEmail(email) {
    var emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;
    return emailRegex.test(email);
  }

  function isValidPhoneNumber(phoneNumber) {
    var phoneRegex = /^(\+\d{1,4}-?)?\d{7,14}$/;
    return phoneRegex.test(phoneNumber);
}

  function isStrongPassword(password) {
    var lowercaseRegex = /[a-z]/;
    var uppercaseRegex = /[A-Z]/;
    var digitRegex = /\d/;
    var specialCharRegex = /[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]/; 

    return (
        lowercaseRegex.test(password) &&
        uppercaseRegex.test(password) &&
        digitRegex.test(password) &&
        specialCharRegex.test(password)
    );
}

  const onSubmit = e => {
    e.preventDefault();
    setError(false);
    let validationPassed = true;

    if (firstname === "" || firstname.length < 2){
      setFirstnameErrorMessage("Firs tname is reqiured, and needs to have more than one letter!");
      validationPassed = false;
    }else{
      setFirstnameErrorMessage("");
    }
    if (lastname === "" || lastname.length < 2){
      setLastnameErrorMessage("Last name is reqiured, and needs to have more than one letter!");
      validationPassed = false;
    }else{
      setLastnameErrorMessage("")
    }
    if (username === "" || username.length < 2){
      setUsernameErrorMessage("Username is reqiured, and needs to have more than one letter!");
      validationPassed = false;
    }else{
      setUsernameErrorMessage("")
    }
    if (!isEmail(email) || email === ""){
      setEmailErrorMessage("You need to provide valid email adress!");
      validationPassed = false;
    }else{
      setEmailErrorMessage("")
    }
    if (!isValidPhoneNumber(phoneNumber) || phoneNumber === ""){
      setPhoneNumberErrorMessage("You need to provide valid phone number!");
      validationPassed = false;
    }else{
      setPhoneNumberErrorMessage("")
    }
    if (dateOfBirth === ""){
      setDateOfBirthErrorMessage("Birthday is reqiured!");
      validationPassed = false;
    }else{
      setDateOfBirthErrorMessage("");
    }
    if (!isStrongPassword(password) || password === ""){
      setPasswordErrorMessage("Password has to contain at least one lowercase letter"+ "<br/>" + "one uppercase letter one digit, and one special character");
      validationPassed = false;
    }else{
      setPasswordErrorMessage("");
    }
    if(password !== passwordRepeat || passwordRepeat === ""){
      setPasswordRepeatErrorMessage("Passwords have to match");
      validationPassed = false;
    }
    else{
      setPasswordRepeatErrorMessage("");
    }

    if (!validationPassed){
      return;
    }

    const body = {
      firstName: firstname,
      lastName: lastname,
      dateOfBirth: dateOfBirth,
      phoneNumber: phoneNumber,
      username: username,
      email: email,
      password: password
    };
    
    const headers = new Headers();
    const url = 'https://localhost:7066/User/register';
    headers.append("Content-Type", "application/json");
    
    fetch(url, {
      method: "POST",
      headers: headers,
      body: JSON.stringify(body),
      credentials: "include"
    })
      .then(response => {
        if (!response.ok) {
          setError(true)
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then(data => {
        console.log(data);
      })
      .catch(error => {
        setError(true)
        console.error("There was a problem with the fetch operation:", error);
      });
  }


  return (
    <div className="container">
      <div className="row">
        <div className="col-sm-10 col-lg-6">
          <form action="submit" onSubmit={onSubmit}>
            <div className="centered-container  centered-container-left">
              <h1>Get to the team</h1>
              <div className="input-container">
                <h3>First name: </h3>
                <input type="text" value={firstname} onChange={e => setFirstname(e.target.value)}/>
                <p className="error-message">{firstnameErrorMessage}</p>
              </div>
              <div className="input-container">
                <h3>Last name: </h3>
                <input type="text" value={lastname} onChange={e => setLastname(e.target.value)}/>
                <p className="error-message">{lastnameErrorMessage}</p>
              </div>
              <div className="input-container">
                <h3>Username: </h3>
                <input type="text" value={username} onChange={e => setUsername(e.target.value)}/>
                <p className="error-message">{usernameErrorMessage}</p>
              </div>
              <div className="input-container">
                <h3>Email: </h3>
                <input type="email" value={email} onChange={e => setEmail(e.target.value)}/>
                <p className="error-message">{emailErrorMessage}</p>
              </div>
              <div className="input-container">
                <h3>Phone number: </h3>
                <input type="text" value={phoneNumber} onChange={e => setPhoneNumber(e.target.value)}/>
                <p className="error-message">{phoneNumberErrorMessage}</p>
              </div>
              <div className="input-container">
                <h3>Birthday: </h3>
                <input type="date" value={dateOfBirth} pattern="\d{4}-\d{2}-\d{2}" onChange={e => setDateOfBirth(e.target.value)} />
                <p className="error-message">{dateOfBirthErrorMessage}</p>
              </div>
              <div className="input-container">
                <h3>Password: </h3>
                <input type="password" value={password} onChange={e => setPassword(e.target.value)}/>
                <p className="error-message">{passwordErrorMessage}</p>
              </div>
              <div className="input-container">
                <h3>Repeat password: </h3>
                <input type="password" value={passwordRepeat} onChange={e => setPasswordRepeat(e.target.value)}/>
                <p className="error-message">{passwordRepeatErrorMessage}</p>
              </div>
              <div className="error-container">
                {error ? <p>User with that username or email adress already exists!</p>:<p></p>}
              </div>
              <div className="button-container">
                <input className="submit-button" type="submit"/>
              </div>
            </div>
          </form>
        </div>
        <div className="col-sm-10 col-lg-6">
          <div className="centered-container video-container centered-container-right">
              <video autoPlay muted loop>
                <source src={video} type="video/mp4"/>
              </video>
          </div>
        </div>
      </div>
    </div>
  )
};

export default RegistartionPage;
