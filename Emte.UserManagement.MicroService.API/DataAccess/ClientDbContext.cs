using System;
using Emte.Core.DataAccess;
using Emte.UserManagement.API.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Emte.UserManagement.BusinessLogic;
using Emte.UserManagement.DomainModels;
using Emte.UserManagement.Models;
using Emte.UserManagement.DataAccess;

namespace Emte.UserManagement.API.DataAccess
{
    public class ClientDbContext : ClientDBContextBase
    {
        private readonly AppConfig _appConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientDbContext(
            DbContextOptions<ClientDbContext> dbContextOptions,
            IOptionsMonitor<AppConfig> config,
            IHttpContextAccessor httpContextAccessor)
        {
            _appConfig = config.CurrentValue;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedClientDetails();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = GetTenantConnectionString();
            optionsBuilder.UseMySql(tenantConnectionString, ServerVersion.AutoDetect(tenantConnectionString));
            base.OnConfiguring(optionsBuilder);
        }

        private string GetTenantConnectionString()
        {
            string defaultConnection = _appConfig.ConnectionStrings?.DefaultConnection!;
            if (_appConfig.MultiTenancyEnabled)
            {
                var tenant = GetCurrentTenantInfo();
                if (tenant == null) { return defaultConnection; }

                var tenantConnectionStringPlaceholder = _appConfig.ConnectionStrings?.ClientConnection;
                return string.Format(tenantConnectionStringPlaceholder!, tenant.Id);
            }

            return defaultConnection;
        }

        private TenantModel? GetCurrentTenantInfo()
        {
            if (_httpContextAccessor != null &&
                _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Items.TryGetValue(Constants.Common.Tenant, out object? value))
            {
                Console.WriteLine($"tenantInfo is - {Newtonsoft.Json.JsonConvert.SerializeObject(value)}");
                return (value as TenantModel) ?? null;
            }

            return null;
        }

        public override Task MigrateAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Client db migrated {GetTenantConnectionString()}");
            return this.Database.MigrateAsync(cancellationToken);
        }
    }
}

