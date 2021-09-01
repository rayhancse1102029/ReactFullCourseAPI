using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.MasterData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Areas.MasterData.Models;
using API.Data.Entity.MasterData;
using API.Service.Helper.Interfaces;
using API.Service.MasterData.Interfaces;

namespace API.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Authorize(Roles = ("Developer, SuperAdmin, Admin, SubAdmin"))]
    public class SliderController : Controller
    {
        private readonly ISliderService iSliderService;
        private readonly IFileSaveService _fileSave;

        public SliderController(IFileSaveService _fileSave, ISliderService iSliderService)
        {
            this.iSliderService = iSliderService;
            this._fileSave = _fileSave;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            SliderViewModel model = new SliderViewModel
            {
                sliders = await iSliderService.GetAllSlider()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SliderViewModel model)
        {

            var result = "error";

            if (!ModelState.IsValid)
            {
                return Json(result);
            }

            string fileName;
            string sliderFileName = String.Empty;

            if (model.img != null)
            {
                string message = _fileSave.SaveImage(out fileName, model.img);

                if (message == "success")
                {
                    sliderFileName = fileName;
                }
                else
                {
                    return Json(result);
                }
            }
            else
            {
                sliderFileName = model.url;
            }

            Slider slider = new Slider
            {
                Id = model.Id,
                name = model.name,
                url = sliderFileName,
                titleOne = model.titleOne,
                titleTwo = model.titleTwo
            };

            bool data = await iSliderService.SaveSlider(slider);

            if (data == true)
            {
                result = "success";
            }

            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> IsActive(int id)
        {
            string msg = "error";

            if (id > 0)
            {
              bool result = await iSliderService.IsActive(id);

              if (result == true)
              {
                  msg = "success";
              }
            }

            return Json(msg);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            string msg = "error";

            if (id > 0)
            {
                bool result = await iSliderService.DeleteSliderById(id);

                if (result == true)
                {
                    msg = "success";
                }
            }

            return Json(msg);
        }
    }
}