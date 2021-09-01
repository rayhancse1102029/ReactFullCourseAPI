using API.Areas.Auth.Models;
using API.Data.Entity;
using API.Data.Entity.Auth;
using API.Services.Auth.Interfaces;
using API.Sevices.AuthServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class NavbarController : Controller
    {
        private readonly INavbarService _navbarService;
        private readonly IUserInfoes _userInfoes;
        private readonly UserManager<ApplicationUser> _userManager;

        public NavbarController(INavbarService _navbarService, IUserInfoes _userInfoes, UserManager<ApplicationUser> _userManager)
        {
            this._navbarService = _navbarService;
            this._userInfoes = _userInfoes;
            this._userManager = _userManager;
        }

        #region Nav Module
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            NavbarViewModel model = new NavbarViewModel
            {
                NavModules = await _navbarService.GetAllNavModule()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(NavbarViewModel model)
        {
            string msg = "error";

            if(!string.IsNullOrEmpty(model.name) && model.shortOrder > 0)
            {
                NavModule module = new NavModule
                {
                    Id = (int)model.id,
                    name = model.name,
                    nameBN = model.nameBN,
                    shortOrder = model.shortOrder,
                    imgClass = model.imgClass
                };
                await _navbarService.SaveNavModule(module);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteNavModuleById(int id)
        {
            string msg = "error";
            if(id > 0)
            {
                bool res = await _navbarService.DeleteNavModuleById(id);
                if(res == true)
                {
                    msg = "success";
                }
            }
            return Json(msg);
        }

        #endregion

        #region Nav Parent
        [HttpGet]
        public async Task<IActionResult> NavParent()
        {
            NavbarViewModel model = new NavbarViewModel
            {
                NavModules = await _navbarService.GetAllNavModule(),
                NavParents = await _navbarService.GetAllNavParent()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NavParent(NavbarViewModel model)
        {
            string msg = "error";

            if (!string.IsNullOrEmpty(model.name) && model.shortOrder > 0 && model.navModuleId > 0)
            {
                NavParent parent = new NavParent
                {
                    Id = (int)model.id,
                    navModuleId = model.navModuleId,
                    name = model.name,
                    nameBN = model.nameBN,
                    shortOrder = model.shortOrder,
                    imgClass = model.imgClass
                };
                await _navbarService.SaveNavParent(parent);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteNavParentById(int id)
        {
            string msg = "error";
            if (id > 0)
            {
                bool res = await _navbarService.DeleteNavParentById(id);
                if (res == true)
                {
                    msg = "success";
                }
            }
            return Json(msg);
        }

        #endregion

        #region Nav Band
        [HttpGet]
        public async Task<IActionResult> NavBand()
        {
            NavbarViewModel model = new NavbarViewModel
            {
                NavModules = await _navbarService.GetAllNavModule(),
                NavParents = await _navbarService.GetAllNavParent(),
                NavBands = await _navbarService.GetAllNavBand()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NavBand(NavbarViewModel model)
        {
            string msg = "error";

            if (!string.IsNullOrEmpty(model.name) && model.shortOrder > 0 && model.navParentId > 0)
            {
                NavBand band = new NavBand
                {
                    Id = (int)model.id,
                    navParentId = model.navParentId,
                    name = model.name,
                    nameBN = model.nameBN,
                    shortOrder = model.shortOrder,
                    imgClass = model.imgClass
                };
                await _navbarService.SaveNavBand(band);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteNavBandById(int id)
        {
            string msg = "error";
            if (id > 0)
            {
                bool res = await _navbarService.DeleteNavBandById(id);
                if (res == true)
                {
                    msg = "success";
                }
            }
            return Json(msg);
        }

        #endregion

        #region Nav Item
        [HttpGet]
        public async Task<IActionResult> NavItem()
        {
            NavbarViewModel model = new NavbarViewModel
            {
                NavModules = await _navbarService.GetAllNavModule(),
                NavParents = await _navbarService.GetAllNavParent(),
                NavBands = await _navbarService.GetAllNavBand(),
                NavItems = await _navbarService.GetAllNavItem()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NavItem(NavbarViewModel model)
        {
            string msg = "error";

            if (!string.IsNullOrEmpty(model.name) && model.shortOrder > 0 && model.navBandId > 0)
            {
                NavItem item = new NavItem
                {
                    Id = (int)model.id,
                    navBandId = model.navBandId,
                    name = model.name,
                    nameBN = model.nameBN,
                    displayOrder = model.shortOrder,
                    imgClass = model.imgClass,
                    area = model.area,
                    controller = model.controller,
                    action = model.action
                };
                await _navbarService.SaveNavItem(item);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteNavItemById(int id)
        {
            string msg = "error";
            if (id > 0)
            {
                bool res = await _navbarService.DeleteNavItemById(id);
                if (res == true)
                {
                    msg = "success";
                }
            }
            return Json(msg);
        }

        #endregion

        #region Page Assign

        [HttpGet]
        public async Task<IActionResult> NavbarAssign(string roleId)
        {
            NavbarViewModel model = new NavbarViewModel
            {
                Roles = await _userInfoes.GetAllRoles(),
                UserAccessPageSPModels = new List<UserAccessPageSPModel>(),
                UserAccessPages = new List<UserAccessPage>(),
                roleId = "no"
            };
            if (!string.IsNullOrEmpty(roleId) && roleId != "no")
            {
                model.roleId = roleId;
                model.UserAccessPageSPModels = await _navbarService.GetAllNavbarsFromSP();
                model.UserAccessPages = await _navbarService.GetAllUserAccessPageByRoleId(roleId);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NavbarAssign(NavbarViewModel model)
        {
            string msg = "error";
            if(!string.IsNullOrEmpty(model.roleId) 
                && (model.moduleIdArray != null 
                || model.parentIdArray != null  
                || model.bandIdArray != null 
                || model.itemdIdArray != null))
            {
                await _navbarService.DeleteAllUserAccessPageByRoleId(model.roleId);
                List<int> moduleIsExists = new List<int>();
                List<int> parentIsExists = new List<int>();
                List<int> bandIsExists = new List<int>();
                
                if (model.itemdIdArray != null)
                {
                    for (int i = 0; i < model.itemdIdArray.Length; i++)
                    {
                        UserAccessPage userAccess = new UserAccessPage
                        {
                            roleId = model.roleId,
                            navItemId = model.itemdIdArray[i]
                        };
                        bool res = await _navbarService.SaveUserAccessPage(userAccess);

                        if(res == true)
                        {
                            NavItem navItem = await _navbarService.GetNavItemById((int)model.itemdIdArray[i]);
                            if (!bandIsExists.Contains((int)navItem.navBandId))
                            {
                                UserAccessPage band = new UserAccessPage
                                {
                                    roleId = model.roleId,
                                    bandId = navItem.navBandId
                                };
                                await _navbarService.SaveUserAccessPage(band);
                                bandIsExists.Add((int)navItem.navBandId);
                            }
                            if (!parentIsExists.Contains((int)navItem.navBand.navParentId))
                            {
                                UserAccessPage parent = new UserAccessPage
                                {
                                    roleId = model.roleId,
                                    parentId = navItem.navBand.navParentId
                                };
                                await _navbarService.SaveUserAccessPage(parent);
                                parentIsExists.Add((int)navItem.navBand.navParentId);
                            }
                            if (!moduleIsExists.Contains((int)navItem.navBand.navParent.navModuleId))
                            {
                                UserAccessPage module = new UserAccessPage
                                {
                                    roleId = model.roleId,
                                    moduleId = navItem.navBand.navParent.navModuleId
                                };
                                await _navbarService.SaveUserAccessPage(module);
                                moduleIsExists.Add((int)navItem.navBand.navParent.navModuleId);
                            }
                        }
                    }
                }
                msg = "success";

                //if (model.moduleIdArray != null)
                //{
                //    for (int i = 0; i < model.moduleIdArray.Length; i++)
                //    {
                //        UserAccessPage userAccess = new UserAccessPage
                //        {
                //            roleId = model.roleId,
                //            moduleId = model.moduleIdArray[i]
                //        };
                //        await _navbarService.SaveUserAccessPage(userAccess);
                //    }
                //}
                //if (model.parentIdArray != null)
                //{
                //    for (int i = 0; i < model.parentIdArray.Length; i++)
                //    {
                //        UserAccessPage userAccess = new UserAccessPage
                //        {
                //            roleId = model.roleId,
                //            parentId = model.parentIdArray[i]
                //        };
                //        await _navbarService.SaveUserAccessPage(userAccess);
                //    }
                //}
                //if (model.bandIdArray != null)
                //{
                //    for (int i = 0; i < model.bandIdArray.Length; i++)
                //    {
                //        UserAccessPage userAccess = new UserAccessPage
                //        {
                //            roleId = model.roleId,
                //            bandId = model.bandIdArray[i]
                //        };
                //        await _navbarService.SaveUserAccessPage(userAccess);
                //    }
                //}
            }
            return Json(msg);
        }

        [HttpPost]
        public IActionResult RoleChaceFunRedirection(string roleId)
        {
            return RedirectToAction("NavbarAssign", "Navbar", new { area = "Auth", roleId });
        }

        #endregion

        public async Task<IActionResult> NavbarLoad()
        {
            NavbarViewModel model = new NavbarViewModel
            { 
                NavModules = new List<NavModule>()
            };

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    string roleId = await _userInfoes.GetRoleIdByUserId(user.Id);
                    model.NavModules = await _navbarService.GetAllUserAccessPageModuleGroupByModuleByRoleId(roleId);
                }
            }

            return PartialView("_NavbarPartial", model);
        }
        public async Task<IActionResult> NavbarBandWiseItemLoadByParentId(int parentId)
        {
            NavbarViewModel model = new NavbarViewModel
            {
                NavModules = new List<NavModule>()
            };

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null && parentId > 0)
                {
                    string roleId = await _userInfoes.GetRoleIdByUserId(user.Id);
                    model.NavBands = await _navbarService.GetAllUserAccessBandGroupByParentByRoleId(roleId, parentId);
                }
            }

            return View(model);
        }


    }
}
