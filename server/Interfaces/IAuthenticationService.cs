using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<IActionResult> Login (LoginDTO loginDTO, IValidationDictionary modelState);
        public Task<IActionResult> Register (RegistrationDTO registrationDTO, IValidationDictionary modelState, String requestUrl);
        public Task<IActionResult> ConfirmUser(string userId, string token);
    }
}
