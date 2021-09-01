using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Auth.Models
{
    public class UserAccessPageSPModel
    {
        //public string roleId { get; set; }
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
    }
}
