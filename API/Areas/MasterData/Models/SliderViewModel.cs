using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using API.Data.Entity.MasterData;

namespace API.Areas.MasterData.Models
{
    public class SliderViewModel
    {
        public int Id { get; set; }
        public IFormFile img { get; set; }
        public string name { get; set; }

        [Required(ErrorMessage = "The Title Field is Required")]
        [StringLength(15, ErrorMessage = "maximum length 15 char")]
        public string titleOne { get; set; }

        [Required(ErrorMessage = "The Title Field is Required")]
        [StringLength(20, ErrorMessage = "maximum length 20 char")]
        public string titleTwo { get; set; }

        public string url { get; set; }
        public IEnumerable<Slider> sliders { get; set; }
    }
}
