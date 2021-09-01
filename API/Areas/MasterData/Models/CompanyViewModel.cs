using Microsoft.AspNetCore.Http;
using API.Data.Entity;
using API.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.MasterData.Models
{
    public class CompanyViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Web { get; set; }
        public IFormFile Logo { get; set; }
        public string LogoUrl { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonDesignation { get; set; }
        public string IncorporateNumber { get; set; }
        public string Tin { get; set; }
        public string Bin { get; set; }
        public int? CompanyTypeId { get; set; }
        public int? BusinessTypeId { get; set; }
        public int? CompanyId { get; set; }
        public int? Username { get; set; }

        public IEnumerable<CompanyType> CompanyTypes { get; set; }
        public IEnumerable<BusinessType> BusinessTypes { get; set; }
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}
