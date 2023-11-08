using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;
using server.Wrappers;
using System.ComponentModel.DataAnnotations;
using server.Services;

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
            StandardServiceResponseDTO<AuthenticationService.ResultData> dto = await _authenticationService.Login(loginDTO, _modelStateWrapper);
            ActionResultWrapper<AuthenticationService.ResultData> response = new ActionResultWrapper<AuthenticationService.ResultData>(dto);
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
            StandardServiceResponseDTO<AuthenticationService.ResultData> dto = await _authenticationService.Register(
                registrationDTO, _modelStateWrapper, $"{Request.Scheme}://{Request.Host}");
            ActionResultWrapper<AuthenticationService.ResultData> response = new ActionResultWrapper<AuthenticationService.ResultData>(dto);
            return response.GetResponse();
        }

        [HttpGet]
        [Route("confirm/{userId}/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmUser([Required] string userId, [Required] string token)
        {
            StandardServiceResponseDTO<AuthenticationService.ResultData> dto = await _authenticationService.ConfirmUser(userId, token);
            ActionResultWrapper<AuthenticationService.ResultData> response = new ActionResultWrapper<AuthenticationService.ResultData>(dto);
            return response.GetResponse();
        }

        [HttpPost]
        [Route("password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PasswordResetRequest([Required] string email)
        {
            StandardServiceResponseDTO<AuthenticationService.ResultData> dto = await _authenticationService.SendPasswordResetRequest(email, $"{Request.Scheme}://{Request.Host}");
            ActionResultWrapper<AuthenticationService.ResultData> response = new ActionResultWrapper<AuthenticationService.ResultData>(dto);
            return response.GetResponse();
        }

        [HttpPut]
        [Route("password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PasswordResetChange([FromBody] ResetPasswordDTO passwordResetDTO)
        {
            StandardServiceResponseDTO<AuthenticationService.ResultData> dto = await _authenticationService.ResetUserPassword(passwordResetDTO, _modelStateWrapper);
            ActionResultWrapper<AuthenticationService.ResultData> response = new ActionResultWrapper<AuthenticationService.ResultData>(dto);
            return response.GetResponse();
        }

        [HttpGet]
        [Route("check-email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyEmailExists([Required] string email)
        {
            StandardServiceResponseDTO<AuthenticationService.ResultData> dto = await _authenticationService.VerifyEmailExists(email);
            ActionResultWrapper<AuthenticationService.ResultData> response = new ActionResultWrapper<AuthenticationService.ResultData>(dto);
            return response.GetResponse();
        }

        [HttpGet]
        [Route("check-username/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyUsernameExists([Required] string userName)
        {
            StandardServiceResponseDTO<AuthenticationService.ResultData> dto = await _authenticationService.VerifyUsernameExists(userName);
            ActionResultWrapper<AuthenticationService.ResultData> response = new ActionResultWrapper<AuthenticationService.ResultData>(dto);
            return response.GetResponse();
        }
    }
}
