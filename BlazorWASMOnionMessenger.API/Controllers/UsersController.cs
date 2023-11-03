using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Users;
using BlazorWASMOnionMessenger.Domain.Common;
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var userDto = await _userService.GetById(userId);
                if (userDto != null)
                {
                    return Ok(userDto);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("page")]
        public async Task<ActionResult<PagedEntities<UserDto>>> GetUsersPage(int page, int pageSize, string orderBy = "", bool? orderType = null, string search = "")
        {
            try
            {
                var pageResult = await _userService.GetPage(page, pageSize, orderBy, orderType ?? false, search);
                return Ok(pageResult);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDto userDto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == userId)
            {
                try
                {
                    await _userService.UpdateUser(userDto, userId);
                    return Ok("User updated successfully.");
                }
                catch (RepositoryException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return Forbid();
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                string token = await _userService.Login(userLoginDto);
                return Ok(new UserResponseDto { IsSuccessful = true, Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return Unauthorized(new UserResponseDto { ErrorMessage = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                string token = await _userService.Register(userRegisterDto);
                return Ok(new UserResponseDto { IsSuccessful = true, Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new UserResponseDto { ErrorMessage = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<ActionResult<UserResponseDto>> ChangePassword([FromBody] UserChangePasswordDto changePasswordDto)
        {
            try
            {
                await _userService.ChangePassword(User.FindFirstValue(ClaimTypes.NameIdentifier), changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                return Ok(new UserResponseDto { IsSuccessful = true });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new UserResponseDto { ErrorMessage = ex.Message });
            }
        }
    }
}
