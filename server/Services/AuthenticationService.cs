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

namespace server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMailUtility _mailUtility;
        private readonly UserManager<User> _userManager;
        private readonly ITokenEncoderUtility _tokenEncoderUtility;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IMailUtility mailUtility, UserManager<User> userManager, ITokenEncoderUtility tokenEncoderUtility)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mailUtility = mailUtility;
            _userManager = userManager;
            _tokenEncoderUtility = tokenEncoderUtility;
        }

        public Task<IActionResult> Login(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new BadRequestResult();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new BadRequestResult();
            }
            code = _tokenEncoderUtility.DecodeToken(code);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Register(RegistrationDTO registrationDTO, IValidationDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new BadRequestResult();
            }

            bool emailExists = await _userRepository.EmailExists(registrationDTO.Email);

            if (emailExists)
            {
                return new BadRequestResult();
            }

            User user = _mapper.Map<User>(registrationDTO);
            var result = await _userRepository.Add(user);
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = _tokenEncoderUtility.EncodeToken(token);
            string url = $"https://localhost:44343/User/EmailConfirmation/{user.Id}/{token}";

            if (result.Succeeded)
            {
                return await SendVerificationEmail(url, user.Email);
            } else
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> SendVerificationEmail(string url, string userEmail)
        {
            MailData mailData = new MailData(new List<string> { userEmail },
                                             "Blea email verification",
                                             $"Click this link to verify your email: {url}");
            bool sent = await _mailUtility.SendEmailAsync(mailData, new CancellationToken());
            if (sent)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
            
        }
    }
}
