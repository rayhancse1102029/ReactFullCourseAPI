using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Areas.MasterData.Models;
using API.Data.Entity;
using API.Data.Entity.MasterData;
using API.Models.MasterData;
using API.Service.Helper.Interfaces;
using API.Services.Auth.Interfaces;
using API.Services.MasterData.Interfaces;

namespace API.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Authorize(Roles = ("Developer, SuperAdmin, Admin, SubAdmin"))]
    public class MasterDataController : Controller
    {
       private readonly IMasterDataService _masterDataService;
       private readonly IFileSaveService _fileSave;
       private readonly ISeedDataService _seedDataService;
       private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MasterDataController(UserManager<ApplicationUser> _userManager, IUserService _userService, ISeedDataService _seedDataService, IFileSaveService _fileSave, IMasterDataService _masterDataService)
        {
            this._masterDataService = _masterDataService;
            this._fileSave = _fileSave;
            this._seedDataService = _seedDataService;
            this._userService = _userService;
            this._userManager = _userManager;
        }
        
        #region Color

        [HttpGet]
        public async Task<IActionResult> Color()
        {

            MasterDataViewModel model = new MasterDataViewModel
            {
                Colors = await _masterDataService.GetAllColor()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Color(MasterDataViewModel model)
        {

            var result = "error";

            if (!ModelState.IsValid)
            {
                return Json(result);
            }

            Color color = new Color
            {
                Id = (int)model.id,
                name = model.name
            };

            bool data = await _masterDataService.SaveColor(color);

            if (data == true)
            {
                result = "success";
            }

            return Json(result);
        }

        public async Task<IActionResult> DeleteColor(int id)
        {
            string msg = "error";

            if (id > 0)
            {
                bool result = await _masterDataService.DeleteColorById(id);

                if (result == true)
                {
                    msg = "success";
                }
            }

            return Json(msg);
        }

        #endregion

        #region Size
        [HttpGet]
        public async Task<IActionResult> Size()
        {

            MasterDataViewModel model = new MasterDataViewModel
            {
                Sizes = await _masterDataService.GetAllSize()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Size(MasterDataViewModel model)
        {

            var result = "error";

            if (!ModelState.IsValid)
            {
                return Json(result);
            }

            Size size = new Size
            {
                Id = (int)model.id,
                name = model.name
            };

            bool data = await _masterDataService.SaveSize(size);

            if (data == true)
            {
                result = "success";
            }

            return Json(result);
        }

        public async Task<IActionResult> DeleteSize(int id)
        {
            string msg = "error";

            if (id > 0)
            {
                bool result = await _masterDataService.DeleteSizeById(id);

                if (result == true)
                {
                    msg = "success";
                }
            }

            return Json(msg);
        }
        #endregion

        #region AddressCategory
        public async Task<IActionResult> AddressCategory()
        {
            AddressCategoryViewModel model = new AddressCategoryViewModel
            {
                addressCategories = await _masterDataService.GetAddressCategory(),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressCategory([FromForm] AddressCategoryViewModel model)
        {
            int validation = await _masterDataService.GetAddressCategoryCheck(model.Id, model.name);
            //return Json(model);
            if (!ModelState.IsValid || validation == 0)
            {
                model.addressCategories = await _masterDataService.GetAddressCategory();
                if (validation == 0)
                {
                    ModelState.AddModelError(string.Empty, "Address Category Name '" + model.name + "' is already exists.");
                }
                return View(model);
            }

            AddressCategory data = new AddressCategory
            {
                Id = model.Id,
                name = model.name,
            };

            await _masterDataService.SaveAddressCategory(data);

            return RedirectToAction(nameof(AddressCategory));
        }

        public async Task<IActionResult> DeleteAddressCategory (int id)
        {
            try
            {
                await _masterDataService.DeleteAddressCategoryById(id);
                return RedirectToAction(nameof(AddressCategory));
            }
            catch
            {
                return RedirectToAction(nameof(AddressCategory));
            }
        }
        #endregion

        #region Brand
        [HttpGet]
        public async Task<IActionResult> Brand()
        {

            BrandViewModel model = new BrandViewModel
            {
                brands = await _masterDataService.GetAllBrand()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Brand(BrandViewModel model)
        {
            var result = "error";
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            string fileName;
            string imgFileName = String.Empty;
            string sliderFileName;
            string sliderImgFileName = String.Empty;

            if (model.img != null)
            {
                string message = _fileSave.SaveImage(out fileName, model.img);

                if (message == "success")
                {
                    imgFileName = fileName;
                }
            }
            if (model.banner != null)
            {
                string message = _fileSave.SaveImage(out sliderFileName, model.banner);

                if (message == "success")
                {
                    sliderImgFileName = sliderFileName;
                }
            }

            if (model.Id > 0)
            {
                Brand findBrand = await _masterDataService.GetBrandById((int)model.Id);

                if (model.img == null)
                {
                    imgFileName = findBrand.url;
                }
                if (model.banner == null)
                {
                    sliderImgFileName = findBrand.banner_url;
                }

            }

            Brand brand = new Brand
            {
                Id = (int)model.Id,
                name = model.name,
                url = imgFileName,
                banner_url = sliderImgFileName,
            };

            bool data = await _masterDataService.SaveBrand(brand);


            return RedirectToAction(nameof(Brand));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            string msg = "error";

            if (id > 0)
            {
                bool result = await _masterDataService.DeleteBrandById(id);

                if (result == true)
                {
                    msg = "success";
                }
            }

            return Json(msg);
        }
        #endregion

        #region Gender
        [HttpGet]
        public async Task<IActionResult> Gender()
        {

            GenderViewModel model = new GenderViewModel
            {
                genders = await _masterDataService.GetAllGender()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Gender(GenderViewModel model)
        {

            var result = "error";

            if (!ModelState.IsValid)
            {
                return Json(result);
            }

            Gender gender = new Gender
            {
                Id = model.id,
                name = model.name
            };

            bool data = await _masterDataService.SaveGender(gender);

            if (data == true)
            {
                result = "success";
            }

            return Json(result);
        }

        public async Task<IActionResult> DeleteGender(int id)
        {
            string msg = "error";

            if (id > 0)
            {
                bool result = await _masterDataService.DeleteGenderById(id);

                if (result == true)
                {
                    msg = "success";
                }
            }

            return Json(msg);
        }
        #endregion

        #region CompanyType
        [HttpGet]
        public async Task<IActionResult> CompanyType()
        {

            CompanyTypeViewModel model = new CompanyTypeViewModel
            {
                CompanyTypes = await _masterDataService.GetAllCompanyType()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CompanyType(CompanyTypeViewModel model)
        {
            var result = "error";
            if (!ModelState.IsValid)
            {
                CompanyType companyType = new CompanyType
                {
                    Id = (int)model.Id,
                    Name = model.Name
                };
                bool data = await _masterDataService.SaveCompanyType(companyType);
                if (data == true)
                {
                    result = "success";
                }
            }
            
            return Json(result);
        }
        public async Task<IActionResult> DeleteCompanyType(int id)
        {
            string msg = "error";
            if (id > 0)
            {
                bool result = await _masterDataService.DeleteCompanyTypeById(id);
                if (result == true)
                {
                    msg = "success";
                }
            }
            return Json(msg);
        }

        #endregion

        #region Company
        [HttpGet]
        [Route("Company")]
        public async Task<IActionResult> Company()
        {
            CompanyViewModel model = new CompanyViewModel
            {
                CompanyTypes = await _masterDataService.GetAllCompanyType(),
                BusinessTypes = await _masterDataService.GetAllBusinessType(),
                Companies = await _masterDataService.GetAllCompany(),
                ApplicationUsers = await _userService.GetAllUser()
            };

            return View(model);
        }
        [HttpPost]
        [Route("Company")]
        public async Task<IActionResult> Company(CompanyViewModel model)
        {
            var result = "error";
            if (ModelState.IsValid)
            {
                string fileName = string.Empty;

                if (model.Logo != null)
                {
                    string message = _fileSave.SaveImage(out fileName, model.Logo);
                    fileName = message == "success" ? fileName : string.Empty;
                }

                Company company = new Company
                {
                    Id = (int)model.Id,
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    Address = model.Address,
                    Web = model.Web,
                    Logo = fileName,
                    ContactPerson = model.ContactPerson,
                    ContactPersonNumber = model.ContactPersonNumber,
                    ContactPersonEmail = model.ContactPersonEmail,
                    ContactPersonDesignation = model.ContactPersonDesignation,
                    IncorporateNumber = model.IncorporateNumber,
                    Tin = model.Tin,
                    Bin = model.Bin,
                    CompanyTypeId = model.CompanyTypeId,
                    BusinessTypeId = model.BusinessTypeId
                };
                int data = await _masterDataService.SaveCompany(company);
                if (data > 0)
                {
                    string res = await _seedDataService.SeedRolesIntoApsNetCompanyRoles(data);
                    result = res == "done" ? "success" : res;
                }
            }

            return Json(result);
        }
        [HttpPost]
        [Route("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            string msg = "error";
            if (id > 0)
            {
                bool result = await _masterDataService.DeleteCompanyById(id);
                if (result == true)
                {
                    msg = "success";
                }
            }
            return Json(msg);
        }
        [HttpPost]
        [Route("LoadCompany")]
        public async Task<IActionResult> LoadCompany()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            List<Company> company = await _masterDataService.GetAllCompany();

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                company = company.AsQueryable().OrderBy(x=>x.Id).ToList();
            }
            else
            {
                company = company.OrderByDescending(x => x.Id).ToList();
            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                company = company.Where(x => x.Name.Contains(searchValue)
                        || (x.ContactPerson != null && x.ContactPerson.Contains(searchValue))
                        || (x.Email != null && x.Email.Contains(searchValue))
                        || (x.Address != null && x.Address.Contains(searchValue))
                        ).ToList();
                //company = company.Where(x => x.CompanyName.Contains(searchValue) || x.ContactPerson.Contains(searchValue)||x.Email.Contains(searchValue)|| x.Address.Contains(searchValue)).ToList();
            }

            company = company.OrderByDescending(i => i.CreatedAt.Value.Date)
                .ThenByDescending(i => i.CreatedAt.Value.TimeOfDay).ToList();
            //total number of rows count     
            recordsTotal = company.Count();

            //Paging     
            var data = company.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpGet]
        [Route("AssignCompanyToUser")]
        public async Task<IActionResult> AssignCompanyToUser(int CompanyId, string Username)
        {
            string res = "error";
            if(CompanyId > 0 && !string.IsNullOrEmpty(Username))
            {
                Company company = await _masterDataService.GetCompanyById(CompanyId);
                ApplicationUser user = await _userService.GetUserInfo(Username);
                if(company != null && user != null)
                {
                    user.CompanyId = CompanyId;
                    bool r = await _userService.UpdateApplicationUser(user);
                    res = r == true ? "success" : res;
                    if(r == true)
                    {
                        user.IsActive = true;
                        await _userManager.UpdateAsync(user);
                        res = "success";
                    }
                }
            }
            return Json(res);
        }
        #endregion


        #region API

        [Route("global/api/GetAllBrand")]
        [HttpGet]
        public async Task<IActionResult> GetAllBrand()
        {

            IEnumerable<Brand> model = await _masterDataService.GetAllBrand();

            return Json(model);
        }

        #endregion
    }
}