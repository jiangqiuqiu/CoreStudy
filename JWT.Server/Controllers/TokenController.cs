using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Server.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private ITokenHelper tokenHelper = null;
        public TokenController(ITokenHelper _tokenHelper)
        {
            tokenHelper = _tokenHelper;
        }

        [HttpGet]
        public IActionResult Get(string code, string pwd)
        {
            User user = TemporaryData.GetUser(code);
            if (null != user && user.Password.Equals(pwd))
            {
                return Ok(tokenHelper.CreateToken(user));
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post()
        {
            return Ok(tokenHelper.RefreshToken(Request.HttpContext.User));
        }
    }
}