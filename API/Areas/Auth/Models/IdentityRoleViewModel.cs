using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Entity;

namespace API.Areas.Auth.Models
{
    public class IdentityRoleViewModel
    {
        public List<string> labels { get; set; }
        public List<string> datas { get; set; }
        public List<string> ids { get; set; }

        public List<ApplicationRole> ApplicationRoleList { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUserList { get; set; }
}
}
