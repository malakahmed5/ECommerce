using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationServices _authenticationService;

        public AuthenticationController(IAuthenticationServices authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authenticationService.LoginAsync(loginDTO);
            return HandleResult(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(result);
        }

        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> IsEmailExist(string email)
        {
            return await _authenticationService.IsEmailExistAsync(email);
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authenticationService.GetCurrentUserByEmailAsync(userEmail);
            return HandleResult(result);
        }

        [HttpGet("{userId}/address")]
        public async Task<ActionResult<UserAddressDTO>> GetUserAddressDetails(string userId)
        {
            var result = await _authenticationService.GetUserAddressDetailsAsync(userId);
            return HandleResult(result);
        }

        [HttpPut("{userId}/address")]
        public async Task<IActionResult> UpdateUserAddress(string userId, UpdateUserAddressDTO userAddressDTO)
        {
            var result = await _authenticationService.UpdateUserAddressAsync(userId, userAddressDTO);
            return HandleResult(result);
        }
    }
}
