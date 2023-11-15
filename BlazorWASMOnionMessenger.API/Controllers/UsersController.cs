using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Users;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorWASMOnionMessenger.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var userDto = await userService.GetById(userId);
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
                var pageResult = await userService.GetPage(page, pageSize, orderBy, orderType, search);
                pageResult.IsSuccessful = true;
                return Ok(pageResult);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new PagedEntities<UserDto>(new List<UserDto>()) { ErrorMessage = ex.Message });
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
                    await userService.UpdateUser(userDto, userId);
                    return Ok(new ResponseDto { IsSuccessful = true });
                }
                catch (RepositoryException ex)
                {
                    return BadRequest(new ResponseDto { ErrorMessage = ex.Message });
                }
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost("changepassword")]
        public async Task<ActionResult<AuthDto>> ChangePassword([FromBody] UserChangePasswordDto changePasswordDto)
        {
            try
            {
                await userService.ChangePassword(User.FindFirstValue(ClaimTypes.NameIdentifier), changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                return Ok(new AuthDto { IsSuccessful = true });
            }
            catch (CustomAuthenticationException ex)
            {
                return BadRequest(new AuthDto { ErrorMessage = ex.Message });
            }
        }
    }
}
