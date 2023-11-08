using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;
using server.Services;

namespace server.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<StandardServiceResponseDTO<AuthenticationService.ResultData>> Login (LoginDTO loginDTO, IValidationDictionary modelState);
        public Task<StandardServiceResponseDTO<AuthenticationService.ResultData>> Register (RegistrationDTO registrationDTO, IValidationDictionary modelState, String requestUrl);
        public Task<StandardServiceResponseDTO<AuthenticationService.ResultData>> ConfirmUser(string userId, string token);
        public Task<StandardServiceResponseDTO<AuthenticationService.ResultData>> SendPasswordResetRequest(string email, string requestUrl);
        public Task<StandardServiceResponseDTO<AuthenticationService.ResultData>> ResetUserPassword(ResetPasswordDTO passwordResetDTO, IValidationDictionary modelState);
        public Task<StandardServiceResponseDTO<AuthenticationService.ResultData>> VerifyEmailExists(string email);
        public Task<StandardServiceResponseDTO<AuthenticationService.ResultData>> VerifyUsernameExists(string username);
    }
}
