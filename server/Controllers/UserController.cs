using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            return Ok();
        }

        [HttpPost]
        [Route("registraion")]
        public IActionResult Registration([FromBody] RegistrationDTO registrationDTO)
        {
            RegistrationService RegistrationService = new RegistrationService(userManager);
            return RegistrationService.Register(registrationDTO);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetUser(string id)
        {
            return Ok();
        }
    }
}
