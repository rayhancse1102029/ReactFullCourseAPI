using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Entity
{
    public class ApplicationUser : IdentityUser
    {

        public string fullName { get; set; }
        public string employeeCode { get; set; }
        public string roleName { get; set; }

        public bool isVerified { get; set; }

        public bool isActive { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? createdAt { get; set; }

        [MaxLength(120)]
        public string createdBy { get; set; }

        public DateTime? updatedAt { get; set; }

        [MaxLength(120)]
        public string updatedBy { get; set; }

        public int? accountType { get; set; }

        public string imgUrl { get; set; }

    }
}
