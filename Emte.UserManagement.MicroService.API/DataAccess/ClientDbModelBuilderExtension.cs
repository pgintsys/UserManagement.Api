using System;
using Emte.UserManagement.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Emte.UserManagement.API.DataAccess
{
    public static class ClientDbModelBuilderExtension
    {
        public static Guid AdminRoleId = new Guid("a9724a8a-216c-4ec5-a382-766e557084b6");
        public static void SeedClientDetails(this ModelBuilder modelBuilder)
        {
            SeedClientRoles(modelBuilder);
        }

        private static void SeedClientRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = AdminRoleId,
                    Name = "SuperAdmin"
                });
        }
    }
}

