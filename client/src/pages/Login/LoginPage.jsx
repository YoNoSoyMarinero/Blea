import React, {useState} from "react"
import './LoginPageStyle.css'
import video from "../../assets/basketball.mp4"


export const LoginPage = (props) => {

  const [email, setEmail] = useState("");
  const [password, setPassword ] = useState("");
  const [error, setError ] = useState(false);

  const onTest = () => {
    let headers = new Headers();
    headers.append("Content-type", "Content-Type", "application/json")
    const url = 'https://localhost:7066/api/Test/test';

    return fetch(url, {
        method: "GET",
        headers: headers,
        credentials: "include"
    })
    .then((response) => {
        if (!response.ok) {
            throw new Error("Request failed with status: " + response.status);
        }
        return response.json();
    })
    .then(data => {
        console.log(data)
    })
    .catch(e => {
        console.log("Error: ", e);
    });
  }


  const onSubmit = () => {
    setError(false);
    const body = {
      email: email,
      password: password
    };
    
    const headers = new Headers();
    const url = 'https://localhost:7066/User/login';
    headers.append("Content-Type", "application/json");
    
    fetch(url, {
      method: "POST",
      headers: headers,
      credentials: "include",
      body: JSON.stringify(body)
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
      <button onClick={onTest}>Test</button>
      <div className="row">
        <div className="col-sm-10 col-lg-6">
          <div className="centered-container-login  centered-container-left">
            <h1>Let's play</h1>
            <div className="input-container">
              <h3>Email: </h3>
              <input type="email" value={email} onChange={e => setEmail(e.target.value)}/>
            </div>
            <div className="input-container">
              <h3>Password: </h3>
              <input type="password" value={password} onChange={e => setPassword(e.target.value)}/>
            </div>
            <div className="error-container">
              {error ? <p>Incorrect password or username</p>:<p></p>}
            </div>
            <div className="reset-password-container">
              <p>Can't remeber your password? Click <a href="">here</a>!</p>
            </div>
            <div className="button-container">
              <button onClick={onSubmit}>Sign in</button>
            </div>
          </div>
        </div>
        <div className="col-sm-10 col-lg-6">
          <div className="centered-container-login video-container centered-container-right">
              <video autoPlay muted loop>
                <source src={video} type="video/mp4"/>
              </video>
              <h1>Blea</h1>
          </div>
        </div>
      </div>
    </div>
  )
};

export default LoginPage;
