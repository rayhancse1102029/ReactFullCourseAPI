using API.Data.Entity;
using API.Data.Entity.Auth;
using API.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Areas.Auth.Models;
using API.Services.MasterData.Interfaces;

namespace API.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class NavbarController : Controller
    {
        private readonly INavbarService _navbarService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IMasterDataService _masterDataService;

        public NavbarController(IMasterDataService _masterDataService, IRoleService _roleService,INavbarService _navbarService, UserManager<ApplicationUser> _userManager)
        {
            this._navbarService = _navbarService;
            this._userManager = _userManager;
            this._roleService = _roleService;
            this._masterDataService = _masterDataService;
        }

        #region Nav Module
        [HttpGet]
        [Route("Module")]
        public async Task<IActionResult> Index()
        {
            NavbarViewModel model = new NavbarViewModel
            {
                NavModules = await _navbarService.GetAllNavModule()
            };
            return View(model);
        }

        [HttpPost]
        [Route("Module")]
        public async Task<IActionResult> Index(NavbarViewModel model)
        {
            string msg = "error";

            if(!string.IsNullOrEmpty(model.Name) && model.ShortOrder > 0)
            {
                NavModule module = new NavModule
                {
                    Id = (int)model.Id,
                    Name = model.Name,
                    ShortOrder = model.ShortOrder,
                    ImgClass = model.ImgClass
                };
                await _navbarService.SaveNavModule(module);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        [Route("DeleteModule")]
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
        [Route("Parent")]
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
        [Route("Parent")]
        public async Task<IActionResult> NavParent(NavbarViewModel model)
        {
            string msg = "error";

            if (!string.IsNullOrEmpty(model.Name) && model.ShortOrder > 0 && model.NavModuleId > 0)
            {
                NavParent parent = new NavParent
                {
                    Id = (int)model.Id,
                    NavModuleId = model.NavModuleId,
                    Name = model.Name,
                    ShortOrder = model.ShortOrder,
                    ImgClass = model.ImgClass
                };
                await _navbarService.SaveNavParent(parent);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        [Route("DeleteParent")]
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
        [Route("Band")]
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
        [Route("Band")]
        public async Task<IActionResult> NavBand(NavbarViewModel model)
        {
            string msg = "error";

            if (!string.IsNullOrEmpty(model.Name) && model.ShortOrder > 0 && model.NavParentId > 0)
            {
                NavBand band = new NavBand
                {
                    Id = (int)model.Id,
                    NavParentId = model.NavParentId,
                    Name = model.Name,
                    ShortOrder = model.ShortOrder,
                    ImgClass = model.ImgClass
                };
                await _navbarService.SaveNavBand(band);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        [Route("DeleteBand")]
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
        [Route("NavItem")]
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
        [Route("NavItem")]
        public async Task<IActionResult> NavItem(NavbarViewModel model)
        {
            string msg = "error";

            if (!string.IsNullOrEmpty(model.Name) && model.ShortOrder > 0 && model.NavBandId > 0)
            {
                NavItem item = new NavItem
                {
                    Id = (int)model.Id,
                    NavBandId = model.NavBandId,
                    Name = model.Name,
                    ShortOrder = model.ShortOrder,
                    ImgClass = model.ImgClass,
                    Area = model.Area,
                    Controller = model.Controller,
                    Action = model.Action
                };
                await _navbarService.SaveNavItem(item);
                msg = "success";
            }
            return Json(msg);
        }

        [HttpGet]
        [Route("DeleteItem")]
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

        #region Page Assign To User      
        [HttpGet]
        [Route("NavbarAssign")]
        public async Task<IActionResult> NavbarAssign(string roleId)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            NavbarViewModel model = new NavbarViewModel
            {
                Roles = User.IsInRole("Developer") ? await _roleService.GetAllApplicationRole()
                    : await _roleService.GetAllActiveRoleByCompnayId((int)user.CompanyId),

                UserAccessPageSPModels = new List<UserAccessPageSPModel>(),
                UserAccessPages = new List<UserAccessPage>(),
                RoleId = "no"
            };
            if (!string.IsNullOrEmpty(roleId) && roleId != "no")
            {
                model.RoleId = roleId;
                model.UserAccessPageSPModels = await _navbarService.GetAllNavbarsFromSP();
                model.UserAccessPages = await _navbarService.GetAllUserAccessPageByRoleId(roleId);
            }

            return View(model);
        }

        [HttpPost]
        [Route("NavbarAssign")]
        public async Task<IActionResult> NavbarAssign(NavbarViewModel model)
        {
            string msg = "error";
            if(!string.IsNullOrEmpty(model.RoleId) 
                && (model.ModuleIdArray != null 
                || model.ParentIdArray != null  
                || model.BandIdArray != null 
                || model.ItemdIdArray != null))
            {
                await _navbarService.DeleteAllUserAccessPageByRoleId(model.RoleId);
                List<int> moduleIsExists = new List<int>();
                List<int> parentIsExists = new List<int>();
                List<int> bandIsExists = new List<int>();
                
                if (model.ItemdIdArray != null)
                {
                    for (int i = 0; i < model.ItemdIdArray.Length; i++)
                    {
                        UserAccessPage userAccessPage = new UserAccessPage
                        {
                            //roleId = model.roleId,
                            NavItemId = model.ItemdIdArray[i]
                        };
                        bool res = await _navbarService.SaveUserAccessPage(userAccessPage);

                        if(res == true)
                        {
                            NavItem navItem = await _navbarService.GetNavItemById((int)model.ItemdIdArray[i]);
                            if (!bandIsExists.Contains((int)navItem.NavBandId))
                            {
                                UserAccessPage band = new UserAccessPage
                                {
                                    //roleId = model.roleId,
                                    BandId = navItem.NavBandId
                                };
                                await _navbarService.SaveUserAccessPage(band);
                                bandIsExists.Add((int)navItem.NavBandId);
                            }
                            if (!parentIsExists.Contains((int)navItem.NavBand.NavParentId))
                            {
                                UserAccessPage parent = new UserAccessPage
                                {
                                    //roleId = model.roleId,
                                    ParentId = navItem.NavBand.NavParentId
                                };
                                await _navbarService.SaveUserAccessPage(parent);
                                parentIsExists.Add((int)navItem.NavBand.NavParentId);
                            }
                            if (!moduleIsExists.Contains((int)navItem.NavBand.NavParent.NavModuleId))
                            {
                                UserAccessPage module = new UserAccessPage
                                {
                                    //roleId = model.roleId,
                                    ModuleId = navItem.NavBand.NavParent.NavModuleId
                                };
                                await _navbarService.SaveUserAccessPage(module);
                                moduleIsExists.Add((int)navItem.NavBand.NavParent.NavModuleId);
                            }
                        }
                    }
                }
                msg = "success";

                //if (model.moduleIdArray != null)
                //{
                //    for (int i = 0; i < model.moduleIdArray.Length; i++)
                //    {
                //        CompanyBasedUserAccessPage userAccess = new CompanyBasedUserAccessPage
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
                //        CompanyBasedUserAccessPage userAccess = new CompanyBasedUserAccessPage
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
                //        CompanyBasedUserAccessPage userAccess = new CompanyBasedUserAccessPage
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
        [Route("RoleChaceFunRedirection")]
        public IActionResult RoleChaceFunRedirection(string roleId)
        {
            return RedirectToAction("NavbarAssign", "Navbar", new { area = "Auth", roleId });
        }

        #endregion

        #region Page Assign to Company
        [HttpGet]
        [Route("CompanyAccessPage")]
        public async Task<IActionResult> CompanyAccessPage(string companyId)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            NavbarViewModel model = new NavbarViewModel
            {
                Companies = await _masterDataService.GetAllCompany(),
                CompanyAccessPageSPModels = new List<UserAccessPageSPModel>(),
                CompnayAccessPages = new List<CompanyAccessPage>(),
                CompanyId = "no"
            };
            if (!string.IsNullOrEmpty(companyId) && companyId != "no")
            {
                model.CompanyId = companyId;
                model.CompanyAccessPageSPModels = await _navbarService.GetAllNavbarsFromSP();
                model.CompnayAccessPages = await _navbarService.GetAllCompanyAccessPageByCompanyId(companyId);
            }

            return View(model);
        }

        [HttpPost]
        [Route("CompanyAccessPage")]
        public async Task<IActionResult> CompanyAccessPage(NavbarViewModel model)
        {
            string msg = "error";
            if (!string.IsNullOrEmpty(model.CompanyId)
                && (model.ModuleIdArray != null
                || model.ParentIdArray != null
                || model.BandIdArray != null
                || model.ItemdIdArray != null))
            {
                List<CompanyAccessPage> ExistingList = (List<CompanyAccessPage>)await _navbarService.GetAllCompanyAccessPageByCompanyId(model.CompanyId);

                if (model.ModuleIdArray != null)
                {
                    for (int i = 0; i < model.ModuleIdArray.Length; i++)
                    {
                        if (!(ExistingList.Where(x => x.ModuleId == model.ModuleIdArray[i]).ToList().Count() > 0))
                        {
                            CompanyAccessPage data = new CompanyAccessPage
                            {
                                CompanyId = (int)Convert.ToInt64(model.CompanyId),
                                ModuleId = model.ModuleIdArray[i]
                            };
                            await _navbarService.SaveCompanyAccessPage(data);
                        }
                    }
                }
                if (model.ParentIdArray != null)
                {
                    for (int i = 0; i < model.ParentIdArray.Length; i++)
                    {
                        if (!(ExistingList.Where(x => x.ParentId == model.ParentIdArray[i]).ToList().Count() > 0))
                        {
                            CompanyAccessPage data = new CompanyAccessPage
                            {
                                CompanyId = (int)Convert.ToInt64(model.CompanyId),
                                ParentId = model.ParentIdArray[i]
                            };
                            await _navbarService.SaveCompanyAccessPage(data);
                        }

                    }
                }
                if (model.BandIdArray != null)
                {
                    for (int i = 0; i < model.BandIdArray.Length; i++)
                    {
                        if (!(ExistingList.Where(x => x.BandId == model.BandIdArray[i]).ToList().Count() > 0))
                        {
                            CompanyAccessPage data = new CompanyAccessPage
                            {
                                CompanyId = (int)Convert.ToInt64(model.CompanyId),
                                BandId = model.BandIdArray[i]
                            };
                            await _navbarService.SaveCompanyAccessPage(data);
                        }
                    }
                }
                if (model.ItemdIdArray != null)
                {
                    for (int i = 0; i < model.ItemdIdArray.Length; i++)
                    {
                        if (!(ExistingList.Where(x => x.NavItemId == model.ItemdIdArray[i]).ToList().Count() > 0))
                        {
                            CompanyAccessPage data = new CompanyAccessPage
                            {
                                CompanyId = (int)Convert.ToInt64(model.CompanyId),
                                NavItemId = model.ItemdIdArray[i]
                            };
                            await _navbarService.SaveCompanyAccessPage(data);
                        }
                    }
                }

                msg = "success";


            }
            return Json(msg);
        }
        [HttpPost]
        [Route("CompanyChaceFunRedirection")]
        public IActionResult CompanyChaceFunRedirection(string companyId)
        {
            return RedirectToAction("CompanyAccessPage", "Navbar", new { area = "Auth", companyId });
        }
        #endregion

        #region Others
        [HttpGet]
        [Route("LoadNavbar")]
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
                    string roleId = User.IsInRole("Developer") ? "Developer" : await _roleService.GetRoleIdByUserId(user.Id);
                    model.NavModules = await _navbarService.GetAllUserAccessPageModuleGroupByModuleByRoleId(roleId);
                }
            }

            return PartialView("_NavbarPartial", model);
        }

        [HttpGet]
        [Route("LoadBand")]
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
                    string roleId = User.IsInRole("Developer") ? "Developer" : await _roleService.GetRoleIdByUserId(user.Id);
                    model.NavBands = await _navbarService.GetAllUserAccessBandGroupByParentByRoleId(roleId, parentId);
                }
            }

            return View(model);
        }
        #endregion


    }
}
