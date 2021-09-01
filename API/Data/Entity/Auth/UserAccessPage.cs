using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Entity.Auth
{
    public class UserAccessPage : Base
    {
        public string roleId { get; set; }
        public IdentityRole role { get; set; }
        public int? moduleId { get; set; }
        public NavModule module { get; set; }
        public int? parentId { get; set; }
        public NavParent parent { get; set; }
        public int? bandId { get; set; }
        public NavBand band { get; set; }
        public int? navItemId { get; set; }
        public NavItem navItem { get; set; }        

    }
}
