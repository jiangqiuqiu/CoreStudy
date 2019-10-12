using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace OptionsBindSample.Controllers
{
    public class HomeController : Controller
    {
        //使用构造函数注入的方式
        //private readonly Class _myClass;
        //public HomeController(IOptions<Class> classAccessor)
        //{
        //    _myClass = classAccessor.Value;
        //}

        //public IActionResult Index()
        //{
        //    return View(_myClass);
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}