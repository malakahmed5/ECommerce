using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> IsEmailExist(string email)
        {
            return await _authenticationService.IsEmailExistAsync(email);
        }

        [Authorize]
        [ProducesResponseType<UserDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authenticationService.GetCurrentUserByEmailAsync(userEmail);
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<UserAddressDTO>> GetUserAddress()
        {
            var email = GetUserEmailFromToken();
            var result = await _authenticationService.GetUserAddressAsync(email);
            return HandleResult(result);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<UserAddressDTO>> UpdateUserAddress(UserAddressDTO userAddressDTO)
        {
            var email = GetUserEmailFromToken();
            var result = await _authenticationService.UpdateUserAddressAsync(email, userAddressDTO);
            return HandleResult(result);
        }
    }
}
