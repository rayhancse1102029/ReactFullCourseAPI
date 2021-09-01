using API.Areas.Auth.Models;
using API.Data.Entity;
using API.Data.Entity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class ReactDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReactDbContext(DbContextOptions<ReactDbContext> options, IHttpContextAccessor _httpContextAccessor) :
            base(options)
        {
            this._httpContextAccessor = _httpContextAccessor;
        }

        #region User Manage
        public DbSet<NavModule> NavModules { get; set; }
        public DbSet<NavParent> NavParents { get; set; }
        public DbSet<NavBand> NavBands { get; set; }
        public DbSet<NavItem> NavItems { get; set; }
        public DbSet<UserAccessPage> UserAccessPages { get; set; }
        public DbQuery<UserAccessPageSPModel> UserAccessPageSPModels { get; set; }
        public DbSet<UserLogHistory> UserLogHistories { get; set; }
        #endregion



        #region Settings Configs
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {

            var entities = ChangeTracker.Entries().Where(x => x.Entity is Base && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = !string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.User?.Identity?.Name)
                ? _httpContextAccessor.HttpContext.User.Identity.Name
                : "Anonymous";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((Base)entity.Entity).createdAt = DateTime.Now.AddHours(-12);
                    ((Base)entity.Entity).createdBy = currentUsername;
                }
                else
                {
                    entity.Property("createdAt").IsModified = false;
                    entity.Property("createdBy").IsModified = false;
                    ((Base)entity.Entity).updatedAt = DateTime.Now.AddHours(-12);
                    ((Base)entity.Entity).updatedBy = currentUsername;
                }

            }
        }
        #endregion
    }
}
