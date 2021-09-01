using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Data.Entity;
using API.Areas.Auth.Models;

namespace API.Areas.SecurityRole.Controllers
{

    [Area("SecurityRole")]
    //[Authorize(Roles = ("Developer, SuperAdmin, Admin, SubAdmin"))]
    public class RolesController : Controller
    {
        private readonly ATMDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RolesController(ATMDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index(string srctext, int page)
        {

            ViewBag.srctext = srctext;

            ViewBag.GetAllRoles = _context.Roles;

            ViewBag.TotalCount = _context.Roles.Count();

            return View();
        }

        // Custom Create Role

        public IActionResult CreateRole()
        {

            ViewBag.GetAllRoles = _context.Roles;
            ViewBag.TotalCount = _context.Roles.Count();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string rolename)
        {
            string msg = "error";
            if (!String.IsNullOrEmpty(rolename))
            {
                var exist = await _roleManager.RoleExistsAsync(rolename);
                if (!exist)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = rolename });
                    msg = "success";
                }
                else
                {
                    msg = "exist";
                }
            }
            return Json(msg);
        }

        public IActionResult AssignRole()
        {
            var roles = _roleManager.Roles;

            List<string> rolelist = new List<string>();

            foreach (var item in roles)
            {
                rolelist.Add(item.Name);
            }

            ViewBag.roles = rolelist;
            ViewBag.msg = "";

            RegisterViewModel model = new RegisterViewModel
            {
                allUsers =  _userManager.Users.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string useremail, string rname)
        {
            string msg = "error";
            if (!string.IsNullOrEmpty(useremail) && !string.IsNullOrEmpty(rname))
            {
                ApplicationUser user = await _userManager.FindByNameAsync(useremail);

                if (!String.IsNullOrEmpty(rname))
                {
                    // previous role delet
                    IEnumerable<string> existingRoleId = _context.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
                    List<string> existingRoles = new List<string>();
                    foreach (var item in existingRoleId)
                    {
                        existingRoles.Add(_context.Roles.Where(x => x.Id == item).Select(x => x.Name).FirstOrDefault());
                    }

                    if (existingRoles.Count() > 0)
                    {
                        if (existingRoles.Contains(rname) == false)
                        {
                            await _userManager.RemoveFromRolesAsync(user, existingRoles);
                            // assign new role
                            IdentityResult result = await _userManager.AddToRoleAsync(user, rname);
                            if (result.Succeeded)
                            {
                                msg = "success";
                            }
                        }
                        else
                        {
                            msg = "exist";
                        }                        
                    }
                    else
                    {                        
                        // assign new role
                        IdentityResult result = await _userManager.AddToRoleAsync(user, rname);
                        if (result.Succeeded)
                        {
                            msg = "success";
                        }
                    }                 
                    
                }               
            }           

            ViewBag.msg = msg;

            var roles = _roleManager.Roles;
            List<string> rolelist = new List<string>();
            foreach (var item in roles)
            {
                rolelist.Add(item.Name);
            }
            ViewBag.roles = rolelist;

            return Json(msg);
        }


    }
}