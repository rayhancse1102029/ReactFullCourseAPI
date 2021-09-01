using API.Data;
using API.Data.Entity;
using API.Areas.Auth.Models;
using API.Areas.User.Models;
using API.Data;
using API.Data.Entity;
using API.Services.Helper.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.User.Controllers
{
    [Area("User")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ReactDbContext _context;
        private readonly IFileSaveService _fileSave;

        public UserController(IFileSaveService _fileSave, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ATMDbContext _context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._context = _context;
            this._fileSave = _fileSave;
        }

        public async Task<IActionResult> Index()
        {

            var model = await (from user in userManager.Users
                               join userRole in _context.UserRoles on user.Id equals userRole.UserId
                               join role in _context.Roles on userRole.RoleId equals role.Id
                               //join g in _context.Genders on user.genderId equals g.Id
                               where role.Name != "GeneralUser"
                               select new ApplicationUser
                               {
                                   UserName = user.UserName,
                                   Email = user.Email,
                                   //firstName = user.firstName,
                                   //lastName = user.lastName,
                                   fullName = user.fullName,
                                   PhoneNumber = user.PhoneNumber,
                                   imgUrl = user.imgUrl,
                                   employeeCode = user.employeeCode,
                                   //Gender = g,
                                   //salary = user.salary,
                                   isVerified = user.isVerified,
                                   isActive = user.isActive,
                                   isDeleted = user.isDeleted,
                                   createdAt = user.createdAt,
                                   createdBy = user.createdBy,
                                   updatedAt = user.updatedAt,
                                   updatedBy = user.updatedBy,
                                   roleName = role.Name
                               }).AsNoTracking().ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> GeneralUser()
        {

            var model = await (from user in userManager.Users
                               join userRole in _context.UserRoles on user.Id equals userRole.UserId
                               join role in _context.Roles on userRole.RoleId equals role.Id
                               //join g in _context.Genders on user.genderId equals g.Id
                               where role.Name == "GeneralUser"
                               select new ApplicationUser
                               {
                                   UserName = user.UserName,
                                   Email = user.Email,
                                   //firstName = user.firstName,
                                   //lastName = user.lastName,
                                   fullName = user.fullName,
                                   PhoneNumber = user.PhoneNumber,
                                   imgUrl = user.imgUrl,
                                   employeeCode = user.employeeCode,
                                   //Gender = g,
                                   //salary = user.salary,
                                   isVerified = user.isVerified,
                                   isActive = user.isActive,
                                   isDeleted = user.isDeleted,
                                   createdAt = user.createdAt,
                                   createdBy = user.createdBy,
                                   updatedAt = user.updatedAt,
                                   updatedBy = user.updatedBy,
                                   roleName = role.Name
                               }).AsNoTracking().ToListAsync();

            return View(model);
        }

        [Route("/api/User/UserDetails/{username}")]
        public async Task<IActionResult> UserDetails(string username)
        {
            ApplicationUser user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();

            List<UserRoleViewModel> findRolesByUser = await (from role in _context.UserRoles.Where(x => x.UserId == user.Id)
                                                             select new UserRoleViewModel
                                                             {
                                                                 userId = role.UserId,
                                                                 roleId = role.RoleId,
                                                                 roleName = roleManager.Roles.Where(x => x.Id == role.RoleId).Select(x => x.Name).FirstOrDefault()
                                                             }).AsNoTracking().ToListAsync();

            user.roleName = findRolesByUser[0].roleName;

            if (string.IsNullOrEmpty(user.roleName))
            {
                user.roleName = "";
            }

            if (string.IsNullOrEmpty(user.employeeCode))
            {
                user.employeeCode = "";
            }

            return Json(user);
        }

        [HttpGet]
        public async Task<IActionResult> UserProfileUpdate()
        {
            RegisterViewModel model = new RegisterViewModel();

            model.user = await _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            List<UserRoleViewModel> findRolesByUser = await (from role in _context.UserRoles.Where(x => x.UserId == model.user.Id)
                                                             select new UserRoleViewModel
                                                             {
                                                                 userId = role.UserId,
                                                                 roleId = role.RoleId,
                                                                 roleName = roleManager.Roles.Where(x => x.Id == role.RoleId).Select(x => x.Name).FirstOrDefault()
                                                             }).AsNoTracking().ToListAsync();

            model.user.roleName = findRolesByUser[0].roleName;

            if (string.IsNullOrEmpty(model.user.roleName))
            {
                model.user.roleName = "";
            }

            if (string.IsNullOrEmpty(model.user.employeeCode))
            {
                model.user.employeeCode = "";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfileUpdate(RegisterViewModel model)
        {
            string msg = "error";

            ApplicationUser user = await userManager.FindByNameAsync(model.Email);

            if (user != null)
            {
                string fileName;
                string fileUrl = string.Empty;

                if (model.img != null)
                {
                    string message = _fileSave.SaveEmpAttachment(out fileName, model.img);
                    if (message == "success")
                    {
                        fileUrl = fileName;
                    }
                }

                //user.firstName = model.firstName;
                //user.lastName = model.lastName;
                //user.fullName = model.firstName + " " + model.lastName;
                //user.PhoneNumber = model.phone;

                if (!string.IsNullOrEmpty(fileUrl))
                {
                    user.imgUrl = fileUrl;
                }

                await userManager.UpdateAsync(user);
                msg = "success";

            }

            return Json(msg);
        }

        [Route("/api/User/LockUser/{username}")]
        [HttpGet]
        public async Task<IActionResult> LockUser(string username)
        {
            string msg = "error";

            ApplicationUser user = await userManager.FindByNameAsync(username);

            if (user.isActive == false)
            {
                user.isActive = true;
            }
            else if (user.isActive == true)
            {
                user.isActive = false;
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
        public async Task<IActionResult> UpdateSalary(UserViewModel model)
        {

            string msg = "error";

            ApplicationUser user = await userManager.FindByNameAsync(model.email);

            //user.salary = (user.salary + model.taka);

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded == true)
            {
                msg = "success";
            }

            return Json(msg);
        }
    }
}
