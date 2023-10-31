using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMOnionMessenger.Domain.DTOs.User
{
    public class UserResponseDto
    {
        public bool IsSuccessful { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
