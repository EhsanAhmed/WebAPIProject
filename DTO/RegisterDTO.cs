using System.ComponentModel.DataAnnotations;

namespace WebAPIProject.DTO
{
    public class RegisterDTO
    {
        //username, email, password,  confirm password).
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        [RegularExpression("[a-z0-9]+@[a-z]+.[a-z]{2,3}")]
        public string Email { get; set; }

    }
}
