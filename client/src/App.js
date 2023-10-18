import { BrowserRouter, Routes, Route } from "react-router-dom";
import {LoginPage} from "./pages/Login/LoginPage";
import {RegistartionPage} from "./pages/Registration/RegistrationPage"


function App() {
  return (
    <div className="App" id = "app-div">
      <BrowserRouter>
        <Routes>
          <Route element={<LoginPage/>} path="/login" />
          <Route element={<RegistartionPage/>} path="/registration"/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
