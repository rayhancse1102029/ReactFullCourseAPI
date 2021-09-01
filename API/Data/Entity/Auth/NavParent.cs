using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Entity.Auth
{
    public class NavParent : Base
    {
        public int? navModuleId { get; set; }
        public NavModule navModule { get; set; }

        [StringLength(150)]
        public string name { get; set; }
        [StringLength(150)]
        public string nameBN { get; set; }
        public int? shortOrder { get; set; }
        //[StringLength(150)]
        //public string isTeam { get; set; }
        [StringLength(150)]
        public string imgClass { get; set; }
    }
}
