using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using API.Areas.Auth.Models;
using API.Controllers;
using API.Data;
using API.Data.Entity;
using API.Service.Helper.Interfaces;
using API.Services.MasterData.Interfaces;
using API.Services.Auth.Interfaces;
using PosSystem.Controllers;

namespace API.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ReactDbContext _context;
        private readonly IMasterDataService _masterDataService;
        private readonly IFileSaveService _fileSave;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> _roleManager,
            ILogger<AccountController> logger,
            ReactDbContext context,
            IFileSaveService _fileSave,
            IMasterDataService _masterDataService,
            IUserService _userService,
            IRoleService _roleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            this._masterDataService = _masterDataService;
            this._roleManager = _roleManager;
            this._fileSave = _fileSave;
            this._userService = _userService;
            this._roleService = _roleService;
        }

        [TempData]
        public string ErrorMessage { get; set; }


        [HttpGet]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            string msg = "";
            if (ModelState.IsValid)
            {
                ApplicationUser userIsExists = await _userManager.FindByNameAsync(model.Phone);
                if (userIsExists != null)
                {
                    msg = "This Phone Number Already Exists please try again!!!";
                }
                else
                {
                    string fileName;
                    string empFileName = String.Empty;
                    string message = _fileSave.SaveUserImage(out fileName, model.Img);

                    if (message == "success")
                    {
                        empFileName = fileName;

                        var user = new ApplicationUser
                        {
                            UserName = model.Phone,
                            //Email = model.Email,
                            FullName = model.FullName,
                            PhoneNumber = model.Phone,
                            EmployeeCode = await _userService.GenerateEmployeeCode(),
                            //genderId = model.genderId,
                            ImgUrl = empFileName,
                            //Address = model.address,
                            IsVerified = false,
                            IsDeleted = false,
                            IsActive = false,
                            CreatedBy = User.Identity.Name,
                            CreatedAt = _roleService.GetDateTimeNow()
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            //_logger.LogInformation("User created a new account with password.");

                            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                            //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                            HttpContext.Session.SetString("profileImgLink", user.ImgUrl);
                            HttpContext.Session.SetString("username", user.FullName);

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            _logger.LogInformation("User created a new account with password.");

                            // POS
                            return View("RegistrationSuccess");
                            // Ecommerce
                            //return RedirectToAction("HybridIndex", "EcommerceWebsite", new { area = "Ecommerce" });
                        }
                        else
                        {
                            msg = "Something is wrong please try Again!!!";
                        }
                    }
                    else
                    {
                        msg = "Error!!! Invalid Image please try Again";
                    }
                }
            }
            else
            {
                msg = model.Img == null ? "Please select your profile image and try Again!!!"
                    : String.IsNullOrEmpty(model.Phone) ? "Please enter username and try Again!!!"
                    : String.IsNullOrEmpty(model.FullName) ? "Please enter full name and try Again!!!"
                    : String.IsNullOrEmpty(model.Password) ? "Please enter password and try Again!!!"
                    : String.IsNullOrEmpty(model.ConfirmPassword) ? "Please enter confirm password and try Again!!!"
                    : "Something is wrong please try Again!!!";
            }
            // If we got this far, something failed, redisplay form
            ViewBag.LoginMessage = msg;
            return View();
            //return RedirectToAction("HybridIndex", "EcommerceWebsite", new {area = "Ecommerce"});
        }

        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.Phone);

                if (user != null)
                {
                    if(!user.IsActive)
                    {
                        return View("RegistrationSuccess");
                    }
                    else if (!user.IsVerified)
                    {
                        return View("RegistrationSuccess");
                    }
                    else if(!user.IsDeleted)
                    {
                        // This doesn't count login failures towards account lockout
                        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                        var result = await _signInManager.PasswordSignInAsync(model.Phone, model.Password, model.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            HttpContext.Session.SetString("profileImgLink", user.ImgUrl);
                            HttpContext.Session.SetString("username", user.FullName);
                            HttpContext.Session.SetString("userId", user.Id);

                            ApplicationUser findApplicationUser = await _userManager.FindByNameAsync(model.Phone);

                            IList<string> getAllRoles = await _userManager.GetRolesAsync(findApplicationUser);
                            // For POS
                            _logger.LogInformation("User logged in.");
                            return RedirectToAction("Index", "Home");

                            // for Ecommerce
                            //if (getAllRoles.Contains("Developer") || getAllRoles.Contains("Admin") || getAllRoles.Contains("SuperAdmin"))
                            //{
                            //    _logger.LogInformation("User logged in.");
                            //    return RedirectToAction("Index", "Home");
                            //}
                            //else
                            //{
                            //    _logger.LogInformation("User logged in.");
                            //    return RedirectToAction("HybridIndex", "EcommerceWebsite", new { area = "Ecommerce" });
                            //}
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToAction(nameof(Lockout));
                        }
                        else
                        {
                            //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            ViewBag.LoginMessage = "Invalid login attempt.";
                            return View(model);
                        }
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, "Sorry !!! Your Account Are Locked");
                        ViewBag.LoginMessage = "Sorry !!! Your Account Are Locked";
                        return View(model);
                    }
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, "Incorrect Username And Password");
                    ViewBag.LoginMessage = "Incorrect Username And Password";
                    return View(model);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            //return RedirectToAction("Index","EcommerceWebsite", new{area= "Ecommerce"});
            return RedirectToAction("Register");
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

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }

        //    var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        //    var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else if (result.IsLockedOut)
        //    {
        //        _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
        //        return RedirectToAction(nameof(Lockout));
        //    }
        //    else
        //    {
        //        _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
        //        ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
        //        return View();
        //    }
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        //{
        //    // Ensure the user has gone through the username & password screen first
        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load two-factor authentication user.");
        //    }

        //    ViewData["ReturnUrl"] = returnUrl;

        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load two-factor authentication user.");
        //    }

        //    var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

        //    var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
        //        return RedirectToAction(nameof(Lockout));
        //    }
        //    else
        //    {
        //        _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
        //        ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
        //        return View();
        //    }
        //}



        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public IActionResult ExternalLogin(string provider, string returnUrl = null)
        //{
        //    // Request a redirect to the external login provider.
        //    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return Challenge(properties, provider);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        //{
        //    if (remoteError != null)
        //    {
        //        ErrorMessage = $"Error from external provider: {remoteError}";
        //        return RedirectToAction(nameof(Login));
        //    }
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction(nameof(Login));
        //    }

        //    // Sign in the user with this external login provider if the user already has a login.
        //    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        return RedirectToAction(nameof(Lockout));
        //    }
        //    else
        //    {
        //        // If the user does not have an account, then ask the user to create an account.
        //        ViewData["ReturnUrl"] = returnUrl;
        //        ViewData["LoginProvider"] = info.LoginProvider;
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await _signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            throw new ApplicationException("Error loading external login information during confirmation.");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await _userManager.AddLoginAsync(user, info);
        //            if (result.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View(nameof(ExternalLogin), model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{userId}'.");
        //    }
        //    var result = await _userManager.ConfirmEmailAsync(user, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //        }

        //        // For more information on how to enable account confirmation and password reset please
        //        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        //        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
        //        await _emailSender.SendEmailAsync(model.Email, "Reset Password",
        //           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
        //        return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string code = null)
        //{
        //    if (code == null)
        //    {
        //        throw new ApplicationException("A code must be supplied for password reset.");
        //    }
        //    var model = new ResetPasswordViewModel { Code = code };
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }
        //    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}
        

        [HttpGet]
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Route("RegSuccess")]
        public IActionResult RegistrationSuccess()
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
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}