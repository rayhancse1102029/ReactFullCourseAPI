using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.MasterData;

namespace API.Areas.MasterData.Models
{
    public class GenderViewModel
    {

        public int id { get; set; }
        public string name { get; set; }


        public IEnumerable<Gender> genders { get; set; }
    }
}
