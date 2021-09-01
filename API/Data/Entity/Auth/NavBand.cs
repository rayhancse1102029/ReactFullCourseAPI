using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Entity.Auth
{
    public class NavBand : Base
    {
        public int? navParentId { get; set; }
        public NavParent navParent { get; set; }

        [StringLength(150)]
        public string name { get; set; }
        [StringLength(150)]
        public string nameBN { get; set; }
        public int? shortOrder { get; set; }
        //[StringLength(150)]
        //public string isTeam { get; set; }
        [StringLength(150)]
        public string imgClass { get; set; }

        [NotMapped]
        public IEnumerable<NavItem> navItems { get; set; }

    }
}
