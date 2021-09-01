using API.Areas.Auth.Models;
using API.Data.Entity;
using API.Data.Entity.Auth;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Sevices.AuthServices.Interfaces
{
    public interface IUserInfoes
    {
        Task<ApplicationUser> GetUserBasicInfoes(string userName);
        //Task<int> GetMaxUserId();
        Task<IEnumerable<ApplicationUser>> GetAllUser();
        Task<ApplicationUser> GetUserInfoByUserEmail(string email);

        #region

        Task<int> SaveRole(RolesViewModel  rolesViewModel);
        Task<IEnumerable<IdentityRole>> GetAllRoles();
        Task<string> GetRoleIdByUserId(string userId);
        //Task<int> DeleteRole();

        #endregion
    }
}
