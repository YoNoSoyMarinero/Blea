using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;
using server.Interfaces;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMailUtility _mailUtility;
        private readonly ITokenEncoderUtility _tokenEncoderUtility;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IMailUtility mailUtility, ITokenEncoderUtility tokenEncoderUtility, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mailUtility = mailUtility;;
            _tokenEncoderUtility = tokenEncoderUtility;
            _configuration = configuration;
        }

        /**
         * @param loginDTO - Contains login information
         * @param modelState - Implements IValidationDictionary interface used to validate user input from Registration DTO 
         */
        public async Task<IActionResult> Login(LoginDTO loginDTO, IValidationDictionary modelState)
        {   
            // Check input in LoginDTO
            if (!modelState.IsValid) return new BadRequestResult();

            // Get the corresponding user by email
            var user = await _userRepository.GetByEmail(loginDTO.Email);

            // Check for the user email, then check if the user is confirmed and lastly check for their password
            if (user == null) return new BadRequestObjectResult("User with this email not found.");

            if (!user.EmailConfirmed) return new UnauthorizedObjectResult("User is not verified, please verify the user.");

            if (!await _userRepository.CheckPassword(user, loginDTO.Password)) return new UnauthorizedObjectResult("Passwords do not match.");

            // Create JWT
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(2), 
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // Send JWT in status 200 response
            return new OkObjectResult(new JWTTokenDTO()
            {
                Username = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        /**
         * @oaram userId - Id of the user to be verified
         * @param code - verification token in the database
         */
        public async Task<IActionResult> ConfirmUser(string userId, string token)
        {   
            var user = await _userRepository.GetById(userId);
            
            // Check if user with this id exists
            if (user == null) return new BadRequestObjectResult("User with provided userId does not exist.");
            
            // Decode the token and confirm the user based on the token
            token = _tokenEncoderUtility.DecodeToken(token);
            var result = await _userRepository.ConfirmUser(user, token);

            // Generate response based on if the user confirmation was successfull
            if (result.Succeeded)
            {
                return new OkResult();
            }
            else
            {
                var errorList = result.Errors.Select(e => e.Description).ToList();
                var errors = string.Join(", ", errorList);
                return new BadRequestObjectResult(errors);
            }
        }
        
        /**
         * @oaram registrationDTO - DTO with registration values (user details)
         * @param modelState - Implements IValidationDictionary interface used to validate user input from Registration DTO
         * @param requestUrl - String holding the value from the frontend url, eg. http://someFrontendUrl so we can concatenate it for verification link
         */
        public async Task<IActionResult> Register(RegistrationDTO registrationDTO, IValidationDictionary modelState, String requestUrl)
        {
            // If the registration details are invalid, return 400 request with the details of invalid info
            if (!modelState.IsValid) return new BadRequestResult();
            
            // Map DTO to the User object
            User user = _mapper.Map<User>(registrationDTO);
            // Add user to the database
            var result = await _userRepository.Add(user);
            
            // If user was inserted into the database successfully, create a verification token and send the link in the email
            if (result.Succeeded)
            {
                string token = await _userRepository.GenerateConfirmationToken(user);
                token = _tokenEncoderUtility.EncodeToken(token);
                string url = $"{requestUrl}/User/confirm/{user.Id}/{token}";

                MailData mailData = new MailData(new List<string> { user.Email }, "Blea email verification", $"Click this link to verify your email: {url}");

                if (await _mailUtility.SendEmailAsync(mailData, new CancellationToken())) return new OkObjectResult("User Successfully created.");

                // If the email was not sent delete the user and respond with status 500
                await _userRepository.Delete(user);
                return new ObjectResult("Verification Email was not sent, please try again.")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };

            } else
            {
                var errorList = result.Errors.Select(e => e.Description).ToList();
                var errors = string.Join(", ", errorList);
                return new ConflictObjectResult(errors);
            }
        }

        /**
         * @param email - Email of the user for the password reset
         * @param requestUrl - the domain and host of the request
         */
        public async Task<IActionResult> SendPasswordResetRequest(string email, string requestUrl)
        {
            var user = await _userRepository.GetByEmail(email);
            
            if (user == null) return new BadRequestObjectResult("User with the given email does not exist");

            var token = await _userRepository.GeneratePasswordResetToken(user);

            token = _tokenEncoderUtility.EncodeToken(token);
            string url = $"{requestUrl}/User/password-reset/{user.Id}/{token}";

            MailData mailData = new MailData(new List<string> { email }, "Blea password reset", $"Click on this link to reset your password: {url}");

            if (await _mailUtility.SendEmailAsync(mailData, new CancellationToken())) return new OkObjectResult("Password Verification link successfully sent.");

            return new ObjectResult(" Email was not sent, please try again.")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        /**
         * @param ResetPasswordDTO  - dto holding token user id and new password for the eset
         */
        public async Task<IActionResult> ResetUserPassword(ResetPasswordDTO passwordResetDTO)
        {
            var user = await _userRepository.GetById(passwordResetDTO.UserId);

            // check if the user exists
            if (user == null) return new BadRequestObjectResult("User with the given Id does not exist");

            // decode the token from the request
            var token = _tokenEncoderUtility.DecodeToken(passwordResetDTO.Token);

            // reset the user password
            var result = await _userRepository.ResetUserPassword(user, token, passwordResetDTO.Password);

            // if the reset was successfull respond with status 200
            if (result.Succeeded) return new OkObjectResult("Password successfully reset!");

            // If the password was not successfull respond with appropriate details
            var errorList = result.Errors.Select(e => e.Description).ToList();
            var errors = string.Join(", ", errorList);
            return new BadRequestObjectResult(errors);
        }
    }
}
