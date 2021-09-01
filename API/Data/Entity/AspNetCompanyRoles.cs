using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Entity;
using API.Data.Entity.MasterData;

namespace API.Data.Entity
{
    public class AspNetCompanyRoles : Base
    {
        public string ApplicationRoleId { get; set; }
        public ApplicationRole ApplicationRole { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
