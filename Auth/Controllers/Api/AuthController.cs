using Auth.DTOs;
using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [Route("all")]
        [HttpGet]
        public IActionResult Index()
        {
            //return NotFound();
            return Ok(_userService.GetAll());
        }
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] AddUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string changePasswordUrl = $"{baseUrl}/Identity/Account/Manage/ChangePassword";

            var addUserModel = new AddUserViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Role = model.Role,
            };
            var result = await _userService.CreateUser(addUserModel, changePasswordUrl);
            if (!result)
                return BadRequest("User registration failed.");

            var jwtSecurityToken = await _authService.CreateJwtToken(model.Email);
            if (jwtSecurityToken == null)
                return StatusCode(500, "Failed to generate authentication token.");

            var authentication = new AuthenticationViewModel
            {
                Email = model.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

            return Ok(authentication);
        }


        [Route("token")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [Route("assignRole")]
        [HttpPost]
        //this is important line that make error
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoleAsync([FromBody] AssignRoleViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.AssignRoleAsync(model);

            if (!result.IsNullOrEmpty())
                return BadRequest(result);
            return Ok(model);

        }
    }
}
