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
        public async Task<ActionResult<PagedEntities<UserDto>>> GetUsersPage(int page, int pageSize, bool orderType, string orderBy = "", string search = "")
        {
            try
            {
                var pageResult = await _userService.GetPage(page, pageSize, orderBy, orderType, search);
                pageResult.IsSuccessful = true;
                return Ok(pageResult);
            }
            catch (RepositoryException ex)
            {
                return BadRequest( new PagedEntities<UserDto>(new List<UserDto>()) { ErrorMessage = ex.Message});
            }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<ResponseDto>> UpdateUser(string userId, [FromBody] UserDto userDto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == userId)
            {
                try
                {
                    await _userService.UpdateUser(userDto, userId);
                    return Ok(new ResponseDto { IsSuccessful = true});
                }
                catch (RepositoryException ex)
                {
                    return BadRequest(new ResponseDto { ErrorMessage = ex.Message});
                }
            }
            else
            {
                return Forbid();
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthDto>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                string token = await _userService.Login(userLoginDto);
                return Ok(new UserAuthDto { IsSuccessful = true, Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return Unauthorized(new UserAuthDto { ErrorMessage = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserAuthDto>> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                string token = await _userService.Register(userRegisterDto);
                return Ok(new UserAuthDto { IsSuccessful = true, Token = token });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new UserAuthDto { ErrorMessage = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<ActionResult<UserAuthDto>> ChangePassword([FromBody] UserChangePasswordDto changePasswordDto)
        {
            try
            {
                await _userService.ChangePassword(User.FindFirstValue(ClaimTypes.NameIdentifier), changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                return Ok(new UserAuthDto { IsSuccessful = true });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new UserAuthDto { ErrorMessage = ex.Message });
            }
        }
    }
}
