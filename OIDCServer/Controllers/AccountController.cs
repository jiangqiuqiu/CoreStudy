using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OIDCServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using OIDCServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http;

namespace OIDCServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly TestUserStore _users;
        public AccountController(TestUserStore users)
        {
            this._users = users;
        }

        //private UserManager<ApplicationUser> _userManager;
        //private SignInManager<ApplicationUser> _signInManager;

        //public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}

        private IActionResult RedirectToLoacl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

       


        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl  = null)
        public IActionResult Register(RegisterViewModel registerViewModel, string returnUrl = null)
        {
            //if (ModelState.IsValid)
            //{
            //    ViewData["ReturnUrl"] = returnUrl;
            //    var identityUser = new ApplicationUser
            //    {
            //        Email = registerViewModel.Email,
            //        UserName = registerViewModel.Email,
            //        NormalizedUserName = registerViewModel.Email,
            //    };

            //    var identityResult = await _userManager.CreateAsync(identityUser, registerViewModel.Password);
            //    if (identityResult.Succeeded)
            //    {
            //        await _signInManager.SignInAsync(identityUser, new AuthenticationProperties { IsPersistent = true });
            //        return RedirectToLoacl(returnUrl);
            //    }
            //    else
            //    {
            //        AddErrors(identityResult);
            //    }
            //}

            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                var user = _users.FindByUsername(loginViewModel.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(loginViewModel.UserName), "User not exists");
                }
                else
                {
                    if(_users.ValidateCredentials(loginViewModel.UserName,loginViewModel.Password))
                    {
                        //�Ƿ��ס
                        var props = new AuthenticationProperties
                        {
                            IsPersistent = true,//��ȡ������һ��ֵ����ֵָʾ�����֤�Ƿ��ǳ־��Եġ�
                            ExpiresUtc =DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))//��ȡ�����������֤�ĵ������ں�ʱ�䡣
                        };

                        //��SignInAsync��Identity4�£�Microsoft.AspNetCore.Http.AuthenticationManagerExtensions
                        //��HttpContext����չ����
                        await HttpContext.SignInAsync(
                           user.SubjectId,
                           user.Username,
                           props);

                        return RedirectToLoacl(returnUrl);
                    }
                    ModelState.AddModelError(nameof(loginViewModel.UserName), "Wrong Passoword");
                }
            }

            return View();
        }

        public IActionResult MakeLogin()
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,"jesse"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();//������������
            return RedirectToAction("Index", "Home");
        }

        

       
    }
}
