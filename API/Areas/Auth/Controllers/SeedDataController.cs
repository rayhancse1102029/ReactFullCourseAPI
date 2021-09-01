using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Services.Auth.Interfaces;

namespace API.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class SeedDataController : Controller
    {
        private readonly ISeedDataService _seedDataService;
        private readonly INavbarService _navbarService;

        public SeedDataController(INavbarService _navbarService, ISeedDataService _seedDataService)
        {
            this._seedDataService = _seedDataService;
            this._navbarService = _navbarService;
        }

        public IActionResult Index()
        {
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> SeedData()
        {
            string msg = "";

            //await _navbarService.DeleteAllMenuRelatedSettings();

            try
            {
                msg += await _seedDataService.SeedRoles();
                msg += "<hr/>";
                msg += await _seedDataService.SeedUser();
                msg += "<hr/>";
                msg += await _seedDataService.SeedRoleAssignToUserRole();
                msg += "<hr/>";
                msg += await _seedDataService.SeedMasterData();
                msg += "<hr/>";
                //msg += await _seedDataService.SeedMenubar(); msg += "<hr/>";
            }
            catch (Exception e)
            {
                msg = e.Message + "<br/>" + e.InnerException;
            }

            return Json(msg);
        }

    }
}