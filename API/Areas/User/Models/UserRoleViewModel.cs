using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.User.Models
{
    public class UserRoleViewModel
    {
        public string userId { get; set; }
        public string roleId { get; set; }

        public string roleName { get; set; }
    }
}
