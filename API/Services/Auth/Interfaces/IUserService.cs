﻿using API.Areas.Auth.Models;
using API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Auth.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUser();
        Task<IEnumerable<ApplicationUser>> GetAllUserByCompanyId(int companyId);
        Task<ApplicationUser> GetUserInfoByUserEmail(string email);
        Task<ApplicationUser> GetUserInfo(string userame);
        Task<bool> UpdateApplicationUser(ApplicationUser user);
        Task<string> GenerateEmployeeCode();
        
    }
}
