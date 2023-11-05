import * as React from 'react'
import RegistrationPage from './pages/Registration/RegistrationPage';
import LoginPage from './pages/Login/LoginPage';
import { BrowserRouter, Routes, Route } from "react-router-dom";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<LoginPage/>} path="/login" />
        <Route element={<RegistrationPage/>} path="/registration"/>
      </Routes>
    </BrowserRouter>
  )
}
