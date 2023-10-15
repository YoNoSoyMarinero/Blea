import { BrowserRouter, Routes, Route } from "react-router-dom";
import {LoginPage} from "./pages/Login/LoginPage";


function App() {
  return (
    <div className="App" id = "app-div">
      <BrowserRouter>
        <Routes>
          <Route element={<LoginPage/>} path="/login" />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
