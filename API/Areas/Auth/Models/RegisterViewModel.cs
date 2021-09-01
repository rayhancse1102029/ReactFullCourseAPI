using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Data.Entity;
using Microsoft.AspNetCore.Http;

namespace API.Areas.Auth.Models
{
    public class RegisterViewModel
    {

        [Required]
        [StringLength(20, ErrorMessage = "maximum 20 char")]
        [DisplayName("Full Name")]
        public string fullName { get; set; }

        [Required]
        [StringLength(6, ErrorMessage = "maximum 4 char")]
        [DisplayName("User Name")]
        public string userName { get; set; }

        public int? accountType { get; set; }

       
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string userId { get; set; }
        public decimal? amount { get; set; }
        public string newPass { get; set; }
        public IFormFile img { get; set; }
        public string applicationUserId { get; set; }

        public IEnumerable<ApplicationUser> allUsers { get; set; } 
        public ApplicationUser user { get; set; } 


    }
}
