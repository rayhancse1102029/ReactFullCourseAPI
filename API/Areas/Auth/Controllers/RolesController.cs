using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Areas.Auth.Models;
using API.Data;
using API.Data.Entity;
using API.Services.Auth.Interfaces;

namespace API.Areas.Auth.Controllers
{
    [Area("Auth")]
    //[Authorize(Roles = ("Developer, SuperAdmin, Admin, SubAdmin"))]
    public class RolesController : Controller
    {
        private readonly ReactDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RolesController(IUserService _userService, IRoleService _roleService, ReactDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            this._roleService = _roleService;
            this._userService = _userService;
        }
        
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            IdentityRoleViewModel model = new IdentityRoleViewModel();
            if (user != null)
            {
                model.ApplicationRoleList = User.IsInRole("Developer") ? await _roleService.GetAllApplicationRole2()
                    : await _roleService.GetAllActiveRoleByCompnayId((int)user.CompanyId);

                model.ApplicationUserList = User.IsInRole("Developer") ? await _userService.GetAllUser()
                    : await _userService.GetAllUserByCompanyId((int)user.CompanyId);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> LoadData()
        {
            CultureInfo culture = new CultureInfo("en-US");
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (User.IsInRole("Developer"))
            {
                List<ApplicationRole> RoleList = await _roleService.GetAllApplicationRole();
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    RoleList = RoleList.Where(m => m.Name.ToLower().Contains(searchValue.ToLower()) ||
                                                   m.Company.Name.ToLower().Contains(searchValue.ToLower()) ||
                                                   m.CreatedBy.ToLower().Contains(searchValue.ToLower())).ToList();

                }
                recordsTotal = RoleList.Count();
                RoleList = RoleList.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = RoleList });
            }
            else
            {
                List<ApplicationRole> RoleList = await _roleService.GetAllRoleByCompnayId((int)user.CompanyId);
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    RoleList = RoleList.Where(m => m.Name.ToLower().Contains(searchValue.ToLower()) ||
                                                   m.CreatedBy.ToLower().Contains(searchValue.ToLower())).ToList();

                }
                recordsTotal = RoleList.Count();
                RoleList = RoleList.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = RoleList });
            }

        }

        public async Task<IActionResult> GetRolesChartData()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            IdentityRoleViewModel model = await _roleService.GetRoleChartData(user.CompanyId > 0 ? (int) user.CompanyId : 0);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            string msg = "Something is wrong please try again!!!.";
            int res = 0;
            if (!String.IsNullOrEmpty(name))
            {
                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                var exist = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToString());
                if (exist == null)
                {
                    var result = await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = name,
                        CompanyId = user.CompanyId,
                        CreatedBy =user.UserName,
                        CreatedAt = _roleService.GetDateTimeNow(),
                        IsActive = true,
                        IsDeleted = false,
                        IsVerified = true

                    });
                    if (result.Succeeded)
                    {
                        ApplicationRole role = await _roleManager.FindByNameAsync(name);
                        AspNetCompanyRoles companyRole = new AspNetCompanyRoles
                        {
                            ApplicationRoleId = role.Id,
                            CompanyId = user.CompanyId
                        };
                        await _roleService.SaveAspNetCompanyRoles(companyRole);
                        msg = "Role " + name + " has been created successfully.";
                        res = 1;
                    }
                }
                else
                {
                    AspNetCompanyRoles isExists =
                        await _roleService.GetAspNetCompanyRolesByRoleIdNCompanyId(exist.Id, (int) user.CompanyId);
                    if (isExists == null)
                    {
                        AspNetCompanyRoles companyRole = new AspNetCompanyRoles
                        {
                            ApplicationRoleId = exist.Id,
                            CompanyId = user.CompanyId
                        };
                      int isDone =  await _roleService.SaveAspNetCompanyRoles(companyRole);
                      if (isDone == 1)
                      {
                          msg = "Role " + name + " has been created successfully.";
                          res = 1;
                      }
                    }
                    msg = "Role " + name + " already exist.";
                }
            }
            else
            {
                msg = "Invalid Role " + name + " please try again.";
            }

            return Json(new { msg, res });
        }
       
        [HttpPost]
        public async Task<IActionResult> AssignRole(string username, string rname)
        {
            string msg = "";
            int res = 0;

            if (!String.IsNullOrEmpty(username) && !string.IsNullOrEmpty(rname))
            {
                ApplicationUser user = await _userManager.FindByNameAsync(username);
                ApplicationRole role = await _roleManager.FindByNameAsync(rname);
               
                if (user != null && role != null)
                {
                    var isExists = await _context.UserRoles.Where(x => x.UserId == user.Id && x.RoleId == role.Id).FirstOrDefaultAsync();
                    if (isExists == null)
                    {
                        IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name);
                        if (result.Succeeded)
                        {
                            msg = rname + " Role has been assigned to User " + user.FullName + "(" + user.UserName + ")";
                            res = 1;
                            user.IsVerified = true;
                            await _userManager.UpdateAsync(user);
                        }
                        else
                        {
                            msg = "Sorry ! Could not assigned role to User " + user.FullName + "(" + user.UserName + ")";

                        }
                    }
                    else
                    {
                        msg = user.FullName + "(" + user.UserName + ")" + "already exists this role [" + role.Name + "]";
                    }
                }
                else
                {
                    msg = user == null ? username + " User does not exist." : "Role " + rname + " does not exist.";
                }
            }
            else
            {
                msg = String.IsNullOrEmpty(username) ? " Error!!! User does not exist." : "Role does not exist.";
            }

            return Json(new {res, msg});
        }


    }
}