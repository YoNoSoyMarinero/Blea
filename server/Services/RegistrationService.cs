using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;
using server.Repository;

namespace server.Services
{
    public class RegistrationService
    {
        private readonly UserManager<User> userManager;

        public RegistrationService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Register(RegistrationDTO registrationDTO)
        {
            UserRepository userRepository = new UserRepository(userManager);
            userRepository.Add(registrationDTO);

            return new OkResult();
        }
    }
}
