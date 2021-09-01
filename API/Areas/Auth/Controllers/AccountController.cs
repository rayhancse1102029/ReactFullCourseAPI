﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using API.Data;
using API.Data.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Areas.Auth.Models;

namespace API.Areas.Auth.Controllers
{

    [Authorize]
    [Area("Auth")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ReactDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> _roleManager,
            ILogger<AccountController> logger,
            ReactDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._roleManager = _roleManager;
            _logger = logger;
            _context = context;

        }

        [TempData]
        public string ErrorMessage { get; set; }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                string msg = "error";



                if (ModelState.IsValid)
                {
                    ApplicationUser findUser = await _userManager.FindByNameAsync(model.userName);

                    if (findUser != null)
                    {
                        msg = "exists";
                    }
                    else
                    {

                        var user = new ApplicationUser
                        {
                            UserName = model.userName,
                            accountType = model.accountType,
                            isVerified = true,
                            isDeleted = false,
                            isActive = true,
                            createdAt = DateTime.Now
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {

                            _logger.LogInformation("User created a new account with password.");

                            msg = "success";

                        }
                        AddErrors(result);
                    }


                }

                // If we got this far, something failed, redisplay form
                //return RedirectToAction("Register", "Account");
                return Json(msg);
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await _userManager.FindByNameAsync(model.userName);

                    if (user != null)
                    {
                        try
                        {
                            var result = await _signInManager.PasswordSignInAsync(model.userName, model.Password, model.RememberMe, lockoutOnFailure: false);
                            if (result.Succeeded)
                            {
                                _logger.LogInformation("User logged in.");

                                return RedirectToAction("Index", "Home");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                return View(model);
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "UnRegistered Account");
                        return View(model);
                    }


                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePass()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePass(RegisterViewModel model)
        {

            if (model.Password != null && model.newPass != null)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user != null)
                {

                   var result =  await _userManager.ChangePasswordAsync(user, model.Password, model.newPass);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");

                        ModelState.AddModelError(string.Empty, "PIN Update Successfully");
                        return RedirectToAction("Login", "Account", new { area = "Auth"});

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "UnRegistered Account");
                    return View(model);
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegistrationSuccessMsg()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}