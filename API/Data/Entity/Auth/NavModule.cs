using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entity.Auth
{
    public class NavModule:Base
    {

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
        public IEnumerable<NavParent> navParents { get; set; }
    }
}
