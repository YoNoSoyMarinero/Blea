using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Email must be at least 8 characters long")]
        [EmailAddress(ErrorMessage = "Email is not in a valid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special symbol, and be at least 8 characters long.")]
        public string Password { get; set; }
    }
}
