using System.ComponentModel.DataAnnotations;

namespace BlazorWASMOnionMessenger.Domain.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username required")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_\W]).+$", ErrorMessage = "Password must contain an uppercase character, a lowercase character, a digit, and a non-alphanumeric character.")]
        public string Password { get; set; } = string.Empty;
    }
}
