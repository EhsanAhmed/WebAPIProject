using Microsoft.AspNetCore.Identity;
using System;

namespace WebAPIProject.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
    }
}
