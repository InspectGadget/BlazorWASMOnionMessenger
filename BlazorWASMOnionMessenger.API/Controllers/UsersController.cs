using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorWASMOnionMessenger.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                string token = await _userService.Login(userLoginDto);
                return Ok(new { Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return Unauthorized(new { ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                string token = await _userService.Register(userRegisterDto);
                return Ok(new { Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto changePasswordDto)
        {
            try
            {
                await _userService.ChangePassword(User.FindFirstValue(ClaimTypes.NameIdentifier), changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                return Ok(new { Message = "Password changed successfully." });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
