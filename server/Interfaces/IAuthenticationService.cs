﻿using Microsoft.AspNetCore.Mvc;
using server.DTOs;

namespace server.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<IActionResult> Login (LoginDTO loginDTO);
        public Task<IActionResult> Register (RegistrationDTO registrationDTO);
    }
}