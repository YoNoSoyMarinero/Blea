using server.DTOs;
using server.Models;
using server.Interfaces;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace server.Services
{
    /// <summary>
    /// This class performs business logic for login, registration, verification, and password resetting.
    /// </summary>
    public class AuthenticationService : Interfaces.IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMailUtility _mailUtility;
        private readonly ITokenEncoderUtility _tokenEncoderUtility;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IMailUtility mailUtility, ITokenEncoderUtility tokenEncoderUtility, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mailUtility = mailUtility;
            _tokenEncoderUtility = tokenEncoderUtility;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Performs user login.
        /// </summary>
        /// <param name="loginDTO">Contains login information.</param>
        /// <param name="modelState">Implements IValidationDictionary interface used to validate user input from Registration DTO.</param>
        public async Task<StandardServiceResponseDTO<ResultData>> Login(LoginDTO loginDTO, IValidationDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.BadRequest,
                    new ResultData { Message = "Invalid Input" }
                );
            }

            var user = await _userRepository.GetByEmail(loginDTO.Email);

            if (user == null)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.NotFound,
                    new ResultData { Message = "User not found." }
                );
            }

            if (!user.EmailConfirmed)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Unauthorized,
                    new ResultData { Message = "User is not verified." }
                );
            }

            if (!await _userRepository.CheckPassword(user, loginDTO.Password))
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Unauthorized,
                    new ResultData { Message = "Incorrect password." }
                );
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return new StandardServiceResponseDTO<ResultData>(
                ResponseType.Success,
                new ResultData { Message = "Log in successful." }
            );
        }

        /// <summary>
        /// Confirms a user's email.
        /// </summary>
        /// <param name="userId">Id of the user to be verified.</param>
        /// <param name="token">Verification token in the database.</param>
        public async Task<StandardServiceResponseDTO<ResultData>> ConfirmUser(string userId, string token)
        {
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.NotFound,
                    new ResultData { Message = "User not found." }
                );
            }

            token = _tokenEncoderUtility.DecodeToken(token);
            var result = await _userRepository.ConfirmUser(user, token);

            if (result.Succeeded)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Success,
                    new ResultData { Message = "User confirmed!" }
                );
            }

            var errorList = result.Errors.Select(e => e.Description).ToList();
            var errors = string.Join(", ", errorList);

            return new StandardServiceResponseDTO<ResultData>(
                ResponseType.BadRequest,
                new ResultData { Message = errors }
            );
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registrationDTO">DTO with registration values (user details).</param>
        /// <param name="modelState">Implements IValidationDictionary interface used to validate user input from Registration DTO.</param>
        /// <param name="requestUrl">String holding the value from the frontend URL.</param>
        public async Task<StandardServiceResponseDTO<ResultData>> Register(RegistrationDTO registrationDTO, IValidationDictionary modelState, string requestUrl)
        {
            if (!modelState.IsValid)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.BadRequest,
                    new ResultData { Message = "Invalid Input" }
                );
            }

            User user = _mapper.Map<User>(registrationDTO);
            var result = await _userRepository.Add(user);

            if (result.Succeeded)
            {
                string token = await _userRepository.GenerateConfirmationToken(user);
                token = _tokenEncoderUtility.EncodeToken(token);
                string url = $"{requestUrl}/User/confirm/{user.Id}/{token}";

                MailData mailData = new MailData(new List<string> { user.Email }, "Blea email verification", $"Click this link to verify your email: {url}");

                if (await _mailUtility.SendEmailAsync(mailData, new CancellationToken()))
                {
                    return new StandardServiceResponseDTO<ResultData>(
                        ResponseType.Success,
                        new ResultData { Message = "Successful registration" }
                    );
                }

                await _userRepository.Delete(user);

                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.InternalServerError,
                    new ResultData { Message = "Registration failed, please try again later." }
                );
            }
            else
            {
                var errorList = result.Errors.Select(e => e.Description).ToList();
                var errors = string.Join(", ", errorList);

                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Conflict,
                    new ResultData { Message = errors }
                );
            }
        }

        /// <summary>
        /// Sends a password reset request to the user.
        /// </summary>
        /// <param name="email">Email of the user for the password reset.</param>
        /// <param name="requestUrl">The domain and host of the request.</param>
        public async Task<StandardServiceResponseDTO<ResultData>> SendPasswordResetRequest(string email, string requestUrl)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.BadRequest,
                    new ResultData { Message = "User not found" }
                );
            }

            var token = await _userRepository.GeneratePasswordResetToken(user);
            token = _tokenEncoderUtility.EncodeToken(token);
            string url = $"{requestUrl}/User/password-reset/{user.Id}/{token}";

            MailData mailData = new MailData(new List<string> { email }, "Blea password reset", $"Click on this link to reset your password: {url}");

            if (await _mailUtility.SendEmailAsync(mailData, new CancellationToken()))
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Success,
                    new ResultData { Message = "Password reset link has been sent." }
                );
            }

            return new StandardServiceResponseDTO<ResultData>(
                ResponseType.InternalServerError,
                new ResultData { Message = "Password reset failed, please try again later." }
            );
        }

        /// <summary>
        /// Resets a user's password.
        /// </summary>
        /// <param name="passwordResetDTO">DTO holding token, user id, and new password for the reset.</param>
        /// <param name="modelState">Implements IValidationDictionary interface used to validate user input.</param>
        public async Task<StandardServiceResponseDTO<ResultData>> ResetUserPassword(ResetPasswordDTO passwordResetDTO, IValidationDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.BadRequest,
                    new ResultData { Message = "Invalid Input" }
                );
            }

            var user = await _userRepository.GetById(passwordResetDTO.UserId);

            if (user == null)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.NotFound,
                    new ResultData { Message = "User not found." }
                );
            }

            var token = _tokenEncoderUtility.DecodeToken(passwordResetDTO.Token);
            var result = await _userRepository.ResetUserPassword(user, token, passwordResetDTO.Password);

            if (result.Succeeded)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Success,
                    new ResultData { Message = "Password successfully reset." }
                );
            }

            var errorList = result.Errors.Select(e => e.Description).ToList();
            var errors = string.Join(", ", errorList);

            return new StandardServiceResponseDTO<ResultData>(
                ResponseType.BadRequest,
                new ResultData { Message = errors }
            );
        }

        /// <summary>
        /// Verifies if an email exists.
        /// </summary>
        /// <param name="email">Email used to check if the user exists.</param>
        public async Task<StandardServiceResponseDTO<ResultData>> VerifyEmailExists(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Success,
                    new ResultData
                    {
                        Message = "Email not found.",
                        EmailFound = "false"
                    }
                );
            }

            return new StandardServiceResponseDTO<ResultData>(
                ResponseType.Success,
                new ResultData
                {
                    Message = "Email found.",
                    EmailFound = "true"
                }
            );
        }

        /// <summary>
        /// Verifies if a username exists.
        /// </summary>
        /// <param name="username">Username used to check if the user exists.</param>
        public async Task<StandardServiceResponseDTO<ResultData>> VerifyUsernameExists(string username)
        {
            var user = await _userRepository.GetByUsername(username);

            if (user == null)
            {
                return new StandardServiceResponseDTO<ResultData>(
                    ResponseType.Success,
                    new ResultData
                    {
                        Message = "Username not found.",
                        UsernameFound = "false"
                    }
                );
            }

            return new StandardServiceResponseDTO<ResultData>(
                ResponseType.Success,
                new ResultData
                {
                    Message = "Username found.",
                    UsernameFound = "true"
                }
            );
        }

        /// <summary>
        /// Class for response data.
        /// </summary>
        public class ResultData
        {
            public string Message { get; set; }
            public string UsernameFound { get; set; }
            public string EmailFound { get; set; }
        }
    }
}
