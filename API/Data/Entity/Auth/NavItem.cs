using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entity.Auth
{
    public class NavItem:Base
    {
        public int? navBandId { get; set; }
        public NavBand navBand { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string name { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string nameBN { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string area { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string controller { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string action { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string imgClass { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string activeLi { get; set; }

        public int? status { get; set; }

        public int? displayOrder { get; set; }       

        [NotMapped]
        public string moduleName { get; set; }
        [NotMapped]
        public string parentName { get; set; }
        [NotMapped]
        public string bandName { get; set; }
    }
}
