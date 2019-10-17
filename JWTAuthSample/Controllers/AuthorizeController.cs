using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTAuthSample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace JWTAuthSample.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizeController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        public AuthorizeController(IOptions<JwtSettings> jwtSettingsAccesser)
        {

            _jwtSettings = jwtSettingsAccesser.Value;
        }

       [HttpPost]
       public IActionResult Token(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!(model.User == "jiang" && model.Password == "123456"))
                {
                    return BadRequest();
                }

                var claim = new Claim[] {
                    new Claim(ClaimTypes.Name,"jiang"),
                    //new Claim("SuperAdminOnly", "true"),//基于Policy的授权，要在Claims里增加这个Policy
                    new Claim(ClaimTypes.Role,"admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    claim,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest();
        }
    }
}