using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIProject.DTO;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private new readonly UserManager<ApplicationUser> User;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> user, IConfiguration configuration)
        {
            this.User = user;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Registeration(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser UserModel = new ApplicationUser();
            UserModel.Email = registerDTO.Email;
            UserModel.UserName = registerDTO.UserName;
           IdentityResult result = await User.CreateAsync(UserModel, registerDTO.Password);
            if(result.Succeeded)
            {
                return Ok("Success");
            }
            else
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return BadRequest(ModelState);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           ApplicationUser UserModel = await User.FindByEmailAsync(loginDTO.Email);
            if(UserModel != null)
            {
                if(await User.CheckPasswordAsync(UserModel, loginDTO.Password))
                {
                    //create token based on claims (...+ jti => unique key for token)
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, UserModel.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, UserModel.Id));

                    var roles = await User.GetRolesAsync(UserModel);
                    foreach(var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecrtKey"]));
                    var token = new JwtSecurityToken(
                        //audience URL
                        audience: configuration["JWT:ValiadAudience"],
                        issuer: configuration["JWT:ValiadIssuer"],
                        expires: DateTime.Now.AddMonths(1),
                        claims: claims,
                        signingCredentials:
                             new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
                        ) ;

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    }) ;
                }
                else
                {
                  return BadRequest("Not Valid pass");
                }
            }
            else
            {
                return BadRequest("user not found");
            }
        }

       

    }
}
