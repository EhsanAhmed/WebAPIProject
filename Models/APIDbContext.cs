using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPIProject.Models;

namespace WebAPI.Models
{
    public class APIDbContext:IdentityDbContext<ApplicationUser>
    {
        public APIDbContext()
        {

        }
      public APIDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
       

    }
}
