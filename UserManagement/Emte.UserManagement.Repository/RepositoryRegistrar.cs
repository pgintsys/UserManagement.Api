using System;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;
using Microsoft.Extensions.DependencyInjection;

namespace Emte.UserManagement.Repository
{
    public static class RepositoryRegistrar
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IRepository<Tenant>, TenantRepository<TenantDbContextBase>>();
            services.AddTransient<IRepository<TenantStatuses>, TenantStatusRepository<TenantDbContextBase>>();


            services.AddTransient<IUserRepository<AppUser>, UserRepository<ClientDBContextBase>>();
			
            services.AddTransient<IRepository<AppRole>, RoleRepository<ClientDBContextBase>>();
            services.AddTransient<IRepository<AppAccess>, AccessRepository<ClientDBContextBase>>();
            services.AddTransient<IRepository<AppUserTokenMap>, UserTokenMapRepository<ClientDBContextBase>>();
            services.AddTransient<IRepository<AppUserResetTokenMap>, UserResetTokenMapRepository<ClientDBContextBase>>();
            services.AddTransient<IRepository<AppRoleAccessMap>, AppRoleAccessMapRepository<ClientDBContextBase>>();
            services.AddTransient<IRepository<AppUserRoleMap>, AppUserRoleMapRepository<ClientDBContextBase>>();
        }
    }
}

