using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Wrappers;
using server.Services;
using Microsoft.AspNetCore.Mvc.Routing;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidationDictionary _modelStateWrapper;
        private readonly IUrlGenerator _urlGenerator;
        
        public UserController(UserManager<User> userManager, IAuthenticationService authenticationService)
        {
            this.userManager = userManager;
            _authenticationService = authenticationService;
            _modelStateWrapper = new ModelStateWrapper(this.ModelState);
            _urlGenerator = new UrlHelperWrapper(Url, HttpContext, "User");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            return Ok();
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationDTO registrationDTO)
        {
            return await _authenticationService.Register(registrationDTO, _modelStateWrapper, _urlGenerator);
        }

        [HttpPost]
        [Route("EmailConfirmation/{userId}/{code}", Name = "EmailConfirmation")]
        public async Task<IActionResult> EmailConfimration(string userId, string code)
        {
            return await _authenticationService.ConfirmEmail(userId, code);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            return Ok();
        }
    }
}
