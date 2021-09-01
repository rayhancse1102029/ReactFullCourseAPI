using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Areas.Auth.Models;
using API.Data.Entity;
using API.Service.Helper.Interfaces;

namespace API.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize(Roles = ("Developer, SuperAdmin, Admin, SubAdmin"))]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFileSaveService _fileSave;

        public UserController(UserManager<ApplicationUser> userManager, IHostingEnvironment _hostingEnvironment, IFileSaveService _fileSave)
        {
            this.userManager = userManager;
            this._hostingEnvironment = _hostingEnvironment;
            this._fileSave = _fileSave;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<ApplicationUser> model = await userManager.Users.ToListAsync();

            return View(model);
        }

        [Route("/api/User/UserDetails/{username}")]
        public async Task<IActionResult> UserDetails(string username)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);

            return Json(user);
        }

        [Route("/api/User/LockUser/{username}")]
        [HttpGet]
        public async Task<IActionResult> LockUser(string username)
        {
            string msg = "error";
            ApplicationUser user = await userManager.FindByNameAsync(username);

            if (user.IsDeleted == false)
            {
                user.IsDeleted = true;
            }
            else if (user.IsDeleted == true)
            {
                user.IsDeleted = false;
            }

           var result = await userManager.UpdateAsync(user);

           if (result.Succeeded)
           {
               msg = "success";
           }

            return Json(msg);
        }

        [Route("/api/User/ForgotPassword/{username}/{password}")]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string username, string password)
        {

            string msg = "error";

            ApplicationUser user = await userManager.FindByNameAsync(username);

            await userManager.RemovePasswordAsync(user);

            var result = await userManager.AddPasswordAsync(user, password);

            if (result.Succeeded == true)
            {
                msg = "success";
            }

            return Json(msg);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfileUpdate(RegisterViewModel model)
        {
            string msg = "error";
            if (model.Img != null)
            {
                ApplicationUser user = await userManager.FindByIdAsync(model.ApplicationUserId);
                if (user != null)
                {
                    string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, user.ImgUrl);
                    if ((System.IO.File.Exists(_imageToBeDeleted)))
                    {
                        System.IO.File.Delete(_imageToBeDeleted);
                    }

                    string fileName;
                    string empFileName = String.Empty;
                    string message = _fileSave.SaveUserImage(out fileName, model.Img);

                    if (message == "success")
                    {
                        user.ImgUrl = fileName;
                        var  res = await userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            msg = "success";
                        }
                    }
                }
            }
           
            return Json(msg);
        }
    }
}