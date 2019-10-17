using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.API.Controllers
{

    // 对此Controller添加了[Authorize] 标识，
    // 表示此Controller的Action被访问时需要进行认证，
    // 而它的名为Get的Action被标识了[AllowAnonymous]，表示此Action的访问可以跳过认证。
    [Authorize]
    [Route("api/[controller]")]    
    public class BookController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return new string[] { "ASP", "C#" };
        }

        // POST api/<controller>
        [HttpPost]
        public JsonResult Post()
        {
            return new JsonResult("Create  Book ...");
        }
    }
}