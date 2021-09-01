using API.Data.Entity.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Entity;
using API.Data.Entity.MasterData;

namespace API.Areas.Auth.Models
{
    public class NavbarViewModel
    {
        public int? Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        public int? ShortOrder { get; set; }
        [StringLength(150)]
        public string ImgClass { get; set; }
        public int? NavModuleId { get; set; }
        public int? NavParentId { get; set; }
        public int? NavBandId { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ActiveLi { get; set; }
        public int? Status { get; set; }
        public int? IsChild { get; set; }

        // user access page
        public string CompanyId { get; set; }
        public string RoleId { get; set; }
        public int? ModuleId { get; set; }
        public string Module { get; set; }
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public int? BandId { get; set; }
        public string Band { get; set; }
        public int? NavItemId { get; set; }
        public string NavItem { get; set; }
        public int? ModuleOrder { get; set; }
        public int? ParentOrder { get; set; }
        public int? BandOrder { get; set; }
        public int? NavItemOrder { get; set; }
        public string ModuleImg { get; set; }
        public string ParentImg { get; set; }
        public string BandImg { get; set; }
        public string NavItemImg { get; set; }

        // others        
        public int?[] ModuleIdArray { get; set; }
        public int?[] ParentIdArray { get; set; }
        public int?[] BandIdArray { get; set; }
        public int?[] ItemdIdArray { get; set; }

        public IEnumerable<NavModule> NavModules { get; set; }
        public IEnumerable<NavParent> NavParents { get; set; }
        public IEnumerable<NavBand> NavBands { get; set; }
        public IEnumerable<NavItem> NavItems { get; set; }
        public IEnumerable<ApplicationRole> Roles { get; set; }
        public IEnumerable<UserAccessPageSPModel> UserAccessPageSPModels { get; set; }
        public IEnumerable<UserAccessPageSPModel> CompanyAccessPageSPModels { get; set; }
        public IEnumerable<CompanyAccessPage> CompnayAccessPages { get; set; }
        public IEnumerable<CompanyBasedUserAccessPage> CompanyBasedUserAccessPages { get; set; }
        public IEnumerable<UserAccessPage> UserAccessPages { get; set; }
        public IEnumerable<Company> Companies { get; set; }
    }
}
