using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Auth.Models
{
    public class RolesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<IdentityRole> IdentityRoles { get; set; }
    }
}
