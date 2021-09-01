using API.Areas.Auth.Models;
using API.Data;
using API.Data.Entity.Auth;
using API.Services.Auth.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Auth
{
    public class NavbarService : INavbarService
    {
        private readonly ReactDbContext _context;

        public NavbarService(ReactDbContext _context)
        {
            this._context = _context;
        }

        #region Module
        public async Task<bool> SaveNavModule(NavModule module)
        {
            if (module.Id != 0)
            {
                _context.NavModules.Update(module);
                await _context.SaveChangesAsync();
                return true;
            }
            await _context.NavModules.AddAsync(module);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<NavModule>> GetAllNavModule()
        {
            return await _context.NavModules.AsNoTracking().ToListAsync();
        }
        public async Task<NavModule> GetNavModuleById(int id)
        {
            return await _context.NavModules.Where(x=>x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteNavModuleById(int id)
        {
            _context.NavModules.Remove(_context.NavModules.Find(id));
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Parent
        public async Task<bool> SaveNavParent(NavParent parent)
        {
            if (parent.Id != 0)
            {
                _context.NavParents.Update(parent);
                await _context.SaveChangesAsync();
                return true;
            }
            await _context.NavParents.AddAsync(parent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<NavParent>> GetAllNavParent()
        {
            return await _context.NavParents.Include(x=>x.navModule).AsNoTracking().ToListAsync();
        }

        public async Task<NavParent> GetNavParentById(int id)
        {
            return await _context.NavParents.Include(x=>x.navModule).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteNavParentById(int id)
        {
            _context.NavParents.Remove(_context.NavParents.Find(id));
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Band
        public async Task<bool> SaveNavBand(NavBand band)
        {
            if (band.Id != 0)
            {
                _context.NavBands.Update(band);
                await _context.SaveChangesAsync();
                return true;
            }
            await _context.NavBands.AddAsync(band);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<NavBand>> GetAllNavBand()
        {
            return await _context.NavBands.Include(x=>x.navParent).Include(x=>x.navParent.navModule).AsNoTracking().ToListAsync();
        }
        public async Task<NavBand> GetNavBandById(int id)
        {
            return await _context.NavBands.Include(x=>x.navParent).Include(x => x.navParent.navModule).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteNavBandById(int id)
        {
            _context.NavBands.Remove(_context.NavBands.Find(id));
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Item
        public async Task<bool> SaveNavItem(NavItem item)
        {
            if (item.Id != 0)
            {
                _context.NavItems.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            await _context.NavItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<NavItem>> GetAllNavItem()
        {
            return await _context.NavItems.Include(x=>x.navBand).Include(x=>x.navBand.navParent).Include(x => x.navBand.navParent.navModule).AsNoTracking().ToListAsync();
        }
        public async Task<NavItem> GetNavItemById(int id)
        {
            return await _context.NavItems.Include(x=>x.navBand).Include(x => x.navBand.navParent).Include(x => x.navBand.navParent.navModule).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteNavItemById(int id)
        {
            _context.NavItems.Remove(_context.NavItems.Find(id));
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region User Access Page
        public async Task<bool> SaveUserAccessPage(UserAccessPage pageAccess)
        {
            if (pageAccess.Id != 0)
            {
                _context.UserAccessPages.Update(pageAccess);
                await _context.SaveChangesAsync();
                return true;
            }
            await _context.UserAccessPages.AddAsync(pageAccess);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<UserAccessPage>> GetAllUserAccessPage()
        {
            return await _context.UserAccessPages.Include(x => x.module).Include(x => x.parent).Include(x => x.band).AsNoTracking().ToListAsync();
        }
        public async Task<UserAccessPage> GetUserAccessPageById(int id)
        {
            return await _context.UserAccessPages.Include(x => x.module).Include(x => x.parent).Include(x => x.band).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteUserAccessPageById(int id)
        {
            _context.UserAccessPages.Remove(_context.UserAccessPages.Find(id));
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Others

        public async Task<IEnumerable<NavParent>> GetAllNavParentByModuleId(int moduleId)
        {
            return await _context.NavParents.Include(x=>x.navModule).Where(x => x.navModuleId == moduleId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<NavBand>> GetAllNavBandByParentId(int parentId)
        {
            return await _context.NavBands.Include(x => x.navParent).Where(x => x.navParentId == parentId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<NavItem>> GetAllNavItemByBandId(int bandId)
        {
            return await _context.NavItems.Include(x => x.navBand).Where(x => x.navBandId == bandId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<UserAccessPageSPModel>> GetAllNavbarsFromSP()
        {
            //return await _context.UserAccessPageSPModels.FromSql($"SP_GetAllNavbars").ToListAsync();
            return  new List<UserAccessPageSPModel>();
        }  
        public async Task<IEnumerable<UserAccessPage>> GetAllUserAccessPageByRoleId(string roleId)
        {
            return await _context.UserAccessPages.Where(x => x.roleId == roleId).ToListAsync();
        }
        public async Task<IEnumerable<NavModule>> GetAllUserAccessPageModuleGroupByModuleByRoleId(string roleId)
        {
            List<NavModule> modules = await _context.UserAccessPages.Where(x => x.roleId == roleId && x.moduleId > 0)
                                                    .Include(x=>x.module).Select(x=>x.module).OrderBy(x=>x.shortOrder).ToListAsync();
            foreach(var item in modules)
            {
                item.navParents = await _context.UserAccessPages.Where(x => x.roleId == roleId && x.parentId > 0)
                                                                .Include(x => x.parent).Include(x=>x.parent.navModule)
                                                                .Where(x=>x.parent.navModuleId == item.Id)
                                                                .Select(x => x.parent).OrderBy(x=>x.shortOrder).ToListAsync();
            }

            return modules;
        }
        public async Task<IEnumerable<NavBand>> GetAllUserAccessBandGroupByParentByRoleId(string roleId, int parentId)
        {
            List<NavBand> parents = await _context.UserAccessPages.Where(x => x.roleId == roleId && x.bandId > 0)
                                                    .Include(x=>x.band).Where(x=>x.band.navParentId == parentId).Select(x => x.band).OrderBy(x=>x.shortOrder).ToListAsync();
            foreach (var item in parents)
            {
                item.navItems = await _context.UserAccessPages.Where(x => x.roleId == roleId && x.navItemId > 0)
                                                                .Include(x => x.navItem).Include(x => x.navItem.navBand)
                                                                .Where(x => x.navItem.navBandId == item.Id)
                                                                .Select(x => x.navItem).OrderBy(x=>x.displayOrder).ToListAsync();
            }

            return parents;
        }

        public async Task<IEnumerable<UserAccessPageSPModel>> GetAllNavbarsWithAssignFlag(string roleId)
        {
            //List<UserAccessPageSPModel> allPages = await _context.UserAccessPageSPModels.FromSql($"SP_GetAllNavbars").ToListAsync();
            //List<UserAccessPage> assigned = await _context.UserAccessPages.Where(x => x.roleId == roleId).ToListAsync();

            //var data = from P in allPages
            //           join M in assigned on P.moduleId equals M.moduleId into MM
            //           from mm in MM.DefaultIfEmpty()
            //           join p in assigned on P.parentId equals p.parentId into PP
            //           from pp in PP.DefaultIfEmpty()
            //           join B in assigned on P.bandId equals B.bandId into BB
            //           from bb in BB.DefaultIfEmpty()
            //           join I in assigned on P.navItemId equals I.navItemId into II
            //           from ii in II.DefaultIfEmpty()
            //           select new
            //           {

            //           }

            //return await _context.UserAccessPageSPModels.FromSql($"SP_GetAllNavbars").ToListAsync();
            return new List<UserAccessPageSPModel>();
        }

        public async Task<bool> DeleteAllUserAccessPageByRoleId(string roleId)
        {
            _context.UserAccessPages.RemoveRange(await _context.UserAccessPages.Where(x => x.roleId == roleId).ToListAsync());
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

    }
}
