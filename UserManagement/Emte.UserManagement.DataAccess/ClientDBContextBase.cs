using System;
using Emte.Core.DataAccess;
using Emte.UserManagement.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Emte.UserManagement.DataAccess
{
    public abstract class ClientDBContextBase : BaseDbContext
    {
        public ClientDBContextBase() { }

        public DbSet<AppUser>? User { get; set; }
        public DbSet<AppUserTokenMap>? UserTokenMap { get; set; }
        public DbSet<AppUserResetTokenMap>? UserResetTokenMap { get; set; }
        public DbSet<AppRole>? Roles { get; set; }
        public DbSet<AppAccess>? AppAccesses { get; set; }
        public DbSet<AppUserRoleMap>? UserRoleMaps { get; set; }
        public DbSet<AppRoleAccessMap>? RoleAccessMaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserTokenMap>().HasOne(au => au.AppUser);
            modelBuilder.Entity<AppUserRoleMap>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<AppUserRoleMap>().HasOne(ur => ur.AppUser).WithMany(ur => ur.Roles).HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<AppUserRoleMap>().HasOne(ur => ur.AppRole).WithMany(ur => ur.Users).HasForeignKey(ur => ur.RoleId);
        }
    }
}

