using API.Areas.Auth.Models;
using API.Data.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Auth.Interfaces
{
    public interface INavbarService
    {
        #region Module
        Task<bool> SaveNavModule(NavModule module);
        Task<IEnumerable<NavModule>> GetAllNavModule();
        Task<NavModule> GetNavModuleById(int id);
        Task<bool> DeleteNavModuleById(int id);
        #endregion

        #region Parent
        Task<bool> SaveNavParent(NavParent parent);
        Task<IEnumerable<NavParent>> GetAllNavParent();
        Task<NavParent> GetNavParentById(int id);
        Task<bool> DeleteNavParentById(int id);
        #endregion

        #region Band
        Task<bool> SaveNavBand(NavBand band);
        Task<IEnumerable<NavBand>> GetAllNavBand();
        Task<NavBand> GetNavBandById(int id);
        Task<bool> DeleteNavBandById(int id);
        #endregion

        #region Item
        Task<bool> SaveNavItem(NavItem item);
        Task<IEnumerable<NavItem>> GetAllNavItem();
        Task<NavItem> GetNavItemById(int id);
        Task<bool> DeleteNavItemById(int id);
        #endregion

        #region User Access Page
        Task<bool> SaveUserAccessPage(UserAccessPage userAccess);
        Task<IEnumerable<UserAccessPage>> GetAllUserAccessPage();
        Task<UserAccessPage> GetUserAccessPageById(int id);
        Task<bool> DeleteUserAccessPageById(int id);
        #endregion

        #region Others

        Task<IEnumerable<NavParent>> GetAllNavParentByModuleId(int moduleId);
        Task<IEnumerable<NavBand>> GetAllNavBandByParentId(int parentId);
        Task<IEnumerable<NavItem>> GetAllNavItemByBandId(int bandId);
        Task<IEnumerable<UserAccessPageSPModel>> GetAllNavbarsFromSP();
        Task<IEnumerable<UserAccessPageSPModel>> GetAllNavbarsWithAssignFlag(string roleId);
        Task<IEnumerable<UserAccessPage>> GetAllUserAccessPageByRoleId(string roleId);
        Task<IEnumerable<NavModule>> GetAllUserAccessPageModuleGroupByModuleByRoleId(string roleId);
        Task<IEnumerable<NavBand>> GetAllUserAccessBandGroupByParentByRoleId(string roleId, int parentId);
        Task<bool> DeleteAllUserAccessPageByRoleId(string roleId);

        #endregion

    }
}
