using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<IActionResult> Login (LoginDTO loginDTO);
        public Task<IActionResult> Register (RegistrationDTO registrationDTO, IValidationDictionary modelState);
        public Task<IActionResult> ConfirmEmail(string userId, string code);

    }
}
