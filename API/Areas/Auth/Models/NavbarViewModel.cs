using API.Data.Entity.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Auth.Models
{
    public class NavbarViewModel
    {
        //module
        public int? id { get; set; }
        [StringLength(150)]
        public string name { get; set; }
        [StringLength(150)]
        public string nameBN { get; set; }
        public int? shortOrder { get; set; }
        [StringLength(150)]
        public string imgClass { get; set; }

        // parent
        public int? navModuleId { get; set; }
        //public string name { get; set; }
        //public string nameBN { get; set; }
        //public int? shortOrder { get; set; }
        //[StringLength(150)]
        //public string imgClass { get; set; }

        // band
        public int? navParentId { get; set; }
        //public string name { get; set; }
        //public string nameBN { get; set; }
        //public int? shortOrder { get; set; }
        //public string imgClass { get; set; }

        // nav item
        public int? navBandId { get; set; }
        //public string name { get; set; }
        //public string nameBN { get; set; }
        public string area { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        //public string imgClass { get; set; }
        public string activeLi { get; set; }
        public int? status { get; set; }
        public int? displayOrder { get; set; }

        // user access page
        public string roleId { get; set; }
        public int? moduleId { get; set; }
        public string module { get; set; }
        public int? parentId { get; set; }
        public string parent { get; set; }
        public int? bandId { get; set; }
        public string band { get; set; }
        public int? navItemId { get; set; }
        public string navItem { get; set; }
        public int? moduleOrder { get; set; }
        public int? parentOrder { get; set; }
        public int? bandOrder { get; set; }
        public int? navItemOrder { get; set; }
        public string moduleImg { get; set; }
        public string parentImg { get; set; }
        public string bandImg { get; set; }
        public string navItemImg { get; set; }

        // others        
        public int?[] moduleIdArray { get; set; }
        public int?[] parentIdArray { get; set; }
        public int?[] bandIdArray { get; set; }
        public int?[] itemdIdArray { get; set; }

        public IEnumerable<NavModule> NavModules { get; set; }
        public IEnumerable<NavParent> NavParents { get; set; }
        public IEnumerable<NavBand> NavBands { get; set; }
        public IEnumerable<NavItem> NavItems { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public IEnumerable<UserAccessPageSPModel> UserAccessPageSPModels { get; set; }
        public IEnumerable<UserAccessPage> UserAccessPages { get; set; }
    }
}
