using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;
using server.Repository;
using server.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidationDictionary _modelState;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<IActionResult> Login(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Register(RegistrationDTO registrationDTO, IValidationDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new BadRequestResult();
            }

            User user = _mapper.Map<User>(registrationDTO);
            await _userRepository.Add(user);
            return new OkResult();
        }
    }
}
