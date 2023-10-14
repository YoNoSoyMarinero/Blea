using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;
using System;
using System.Web;
using server.Interfaces;
using AutoMapper;
using System.Security.Policy;
using server.Utilites;
using Org.BouncyCastle.Asn1.Ocsp;
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
         */
        public async Task<IActionResult> Login(LoginDTO loginDTO, IValidationDictionary modelState)
        {
            if (!modelState.IsValid) return new BadRequestObjectResult( modelState.GetErrors());

            var user = await _userRepository.GetByEmail(loginDTO.Email);

            if (user == null) return new BadRequestObjectResult("User with this email not found.");

            if (!user.EmailConfirmed) return new UnauthorizedObjectResult("User is not verified, please verify the user.");

            if (!await _userRepository.CheckPassword(user, loginDTO.Password)) return new UnauthorizedObjectResult("Passwords do not match.");

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
            if (userId == null || token == null) return new BadRequestObjectResult("UserId or verification token is invalid.");
            
            var user = await _userRepository.GetById(userId);

            if (user == null) return new BadRequestObjectResult("User with provided userId does not exist.");
            
            token = _tokenEncoderUtility.DecodeToken(token);
            var result = await _userRepository.ConfirmUser(user, token);

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
         * @param registrationDTO - DTO with registration values (user details)
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
                MailData mailData = new MailData(new List<string> { user.Email },
                                             "Blea email verification",
                                             $"Click this link to verify your email: {url}");

                if (await _mailUtility.SendEmailAsync(mailData, new CancellationToken()))
                {
                    return new OkObjectResult("User Successfully created.");
                } else
                {
                    await _userRepository.Delete(user);
                    return new ObjectResult("Verification Email was not sent, please try again.")
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
                
            } else
            {
                var errorList = result.Errors.Select(e => e.Description).ToList();
                var errors = string.Join(", ", errorList);
                return new ConflictObjectResult(errors);
            }
        }
    }
}
