﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Wrappers;
using server.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Web;

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


        //User login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            return await _authenticationService.Login(loginDTO, _modelStateWrapper);
        }


        //User registration
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationDTO registrationDTO)
        {
            return await _authenticationService.Register(registrationDTO, _modelStateWrapper, $"{Request.Scheme}://{Request.Host}");
        }

        //Sends confirmation link after registration
        [HttpPost]
        [Route("confirm/{userId}/{token}")]
        public async Task<IActionResult> ConfirmUser(string userId, string token)
        {
            return await _authenticationService.ConfirmUser(userId, token);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            return Ok();
        }
    }
}
