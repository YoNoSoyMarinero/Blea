using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;
using server.Repository;
using server.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Policy;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System;
using server.Utilites;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;

namespace server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMailUtility _mailUtility;
        private readonly UserManager<User> _userManager;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IMailUtility mailUtility, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mailUtility = mailUtility;
            _userManager = userManager;
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

        public async Task<IActionResult> Register(RegistrationDTO registrationDTO, IValidationDictionary modelState, IUrlGenerator urlGenerator)
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

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = urlGenerator.GenerateVerificationLink("ConfirmEmail", token, user.Email);
                System.Console.WriteLine(callbackUrl);
            }

            return new OkResult();
        }
    }
}
