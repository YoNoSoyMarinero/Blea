import React, {useState} from "react"
import './LoginPageStyle.css'
import video from "../../images/basketball.mp4"


export const LoginPage = (props) => {

  const [email, setEmail] = useState("");
  const [password, setPassword ] = useState("");
  const [error, setError ] = useState(false);


  const onSubmit = () => {
    console.log(email, password)
  }


  return (
    <div className="container">
      <div className="row">
        <div className="col-sm-10 col-lg-6">
          <div className="centered-container  centered-container-left">
            <h1>Let's get you into the game</h1>
            <div className="input-container">
              <h3>Email: </h3>
              <input type="email" />
            </div>
            <div className="input-container" value={email} onChange={e => setEmail(e.target.value)}>
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
          <div className="centered-container video-container centered-container-right">
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
