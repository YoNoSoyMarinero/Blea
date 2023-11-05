using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<StandardServiceResponseDTO> Login (LoginDTO loginDTO, IValidationDictionary modelState);
        public Task<StandardServiceResponseDTO> Register (RegistrationDTO registrationDTO, IValidationDictionary modelState, String requestUrl);
        public Task<StandardServiceResponseDTO> ConfirmUser(string userId, string token);
        public Task<StandardServiceResponseDTO> SendPasswordResetRequest(string email, string requestUrl);
        public Task<StandardServiceResponseDTO> ResetUserPassword(ResetPasswordDTO passwordResetDTO, IValidationDictionary modelState);
        public Task<StandardServiceResponseDTO> VerifyEmailExists(string email);
        public Task<StandardServiceResponseDTO> VerifyUsernameExists(string username);
    }
}
