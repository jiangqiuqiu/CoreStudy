using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MvcCookieAuthSample.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {            
            //如果HttpContext.User.Identity.IsAuthenticated为true，
            //或者HttpContext.User.Claims.Count()大于0表示用户已经登录
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //这里通过 HttpContext.User.Claims 可以将我们在Login这个Action中存储到cookie中的所有
                //claims键值对都读出来，比如我们刚才定义的UserName的值就在这里读取出来了
                var userName = HttpContext.User.Claims.First().Value;
                ViewBag.UserName = userName;
            }

            return View();
        }
    }
}