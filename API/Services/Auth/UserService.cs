using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Areas.Auth.Models;
using API.Data;
using API.Data.Entity;
using API.Services.Auth.Interfaces;

namespace API.Services.Auth
{
    public class UserService : IUserService
    {
        private readonly ReactDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(RoleManager<ApplicationRole> _roleManager, ReactDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUser()
        {
            var model = await _context.Users.Include(x=>x.Company).ToListAsync();

            return model;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUserByCompanyId(int companyId)
        {
            var model = await _context.Users.Where(x=>x.CompanyId == companyId).Include(x=>x.Company).ToListAsync();

            return model;
        }
        public async Task<ApplicationUser> GetUserInfoByUserEmail(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;

        }
        public async Task<ApplicationUser> GetUserInfo(string userame)
        {
            ApplicationUser user = await _context.Users.Where(x => x.UserName == userame).FirstOrDefaultAsync();
            return user;
        }
        public async Task<bool> UpdateApplicationUser(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> GenerateEmployeeCode()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int maxId = await _context.Users.CountAsync() + 1;

            string getEmpCode = year + "" + month + "" + maxId;

            return getEmpCode;
        }

       

    }
}
