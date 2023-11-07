using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Authentication;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMOnionMessenger.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login([FromBody] LoginDto userLoginDto)
        {
            try
            {
                string token = await _authService.Login(userLoginDto);
                return Ok(new AuthDto { IsSuccessful = true, Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return Unauthorized(new AuthDto { ErrorMessage = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthDto>> Register([FromBody] RegisterDto userRegisterDto)
        {
            try
            {
                string token = await _authService.Register(userRegisterDto);
                return Ok(new AuthDto { IsSuccessful = true, Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new AuthDto { ErrorMessage = ex.Message });
            }
        }
    }
}
