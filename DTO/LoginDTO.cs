using System.ComponentModel.DataAnnotations;

namespace WebAPIProject.DTO
{
    public class LoginDTO
    {
        [Required]
        [RegularExpression("[a-z0-9]+@[a-z]+.[a-z]{2,3}")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
