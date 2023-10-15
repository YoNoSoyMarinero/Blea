using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Wrappers;
using System.ComponentModel.DataAnnotations;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidationDictionary _modelStateWrapper;

        public UserController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _modelStateWrapper = new ModelStateWrapper(this.ModelState);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            StandardServiceResponseDTO dto = await _authenticationService.Login(loginDTO, _modelStateWrapper);
            ActionResultWrapper response = new ActionResultWrapper(dto);
            return response.GetResponse();
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationDTO registrationDTO)
        {
            StandardServiceResponseDTO dto = await _authenticationService.Register(registrationDTO, _modelStateWrapper, $"{Request.Scheme}://{Request.Host}");
            ActionResultWrapper response = new ActionResultWrapper(dto);
            return response.GetResponse();
        }

        [HttpGet]
        [Route("confirm/{userId}/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmUser([Required] string userId, [Required] string token)
        {
            StandardServiceResponseDTO dto = await _authenticationService.ConfirmUser(userId, token);
            ActionResultWrapper response = new ActionResultWrapper(dto);
            return response.GetResponse();
        }

        [HttpPost]
        [Route("password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PasswordResetRequest([Required] string email)
        {
            StandardServiceResponseDTO dto = await _authenticationService.SendPasswordResetRequest(email, $"{Request.Scheme}://{Request.Host}");
            ActionResultWrapper response = new ActionResultWrapper(dto);
            return response.GetResponse();
        }

        [HttpPut]
        [Route("password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PasswordResetChange([FromBody] ResetPasswordDTO passwordResetDTO)
        {
            StandardServiceResponseDTO dto = await _authenticationService.ResetUserPassword(passwordResetDTO, _modelStateWrapper);
            ActionResultWrapper response = new ActionResultWrapper(dto);
            return response.GetResponse();
        }
    }
}
