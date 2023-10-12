using System;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using Emte.Core.API;
using Emte.Core.Authentication.Contract;
using Emte.Core.Authentication.Impl;
using Emte.Core.DataAccess;
using Emte.Core.DataAccess.Impl;
using Emte.UserManagement.API.DataAccess;
using Emte.UserManagement.BusinessLogic;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Emte.UserManagement.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Host DB
            services.AddDbContext<TenantDbContext>();

            // Client DB
            services.AddDbContext<ClientDbContext>();

            RegisterCore(services);

            BusinessLogic.BusinessLogicRegistrar.Register(services);
            Repository.RepositoryRegistrar.Register(services);
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IAuthenticationService<AppUser, AppUserTokenMap>, AuthenticationService<AppUser, AppUserTokenMap>>();
            services.AddTransient<IRefreshTokenService<AppUserTokenMap, AppUser>, RefreshTokenService<AppUserTokenMap, AppUser>>();
            services.AddTransient<IAuthenticationService<AppUser, AppUserResetTokenMap>, AuthenticationService<AppUser, AppUserResetTokenMap>>();
            services.AddTransient<IRefreshTokenService<AppUserResetTokenMap, AppUser>, RefreshTokenService<AppUserResetTokenMap, AppUser>>();
            services.AddTransient<IResetPasswordService<AppUserResetTokenMap, AppUser>, ResetPasswordService<AppUserResetTokenMap, AppUser>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void RegisterCore(IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IEmailService, GmailService>();
            services.AddScoped<TenantDbContextBase, TenantDbContext>();
            services.AddScoped<ClientDBContextBase, ClientDbContext>();
            services.AddScoped<IEntityService<TenantDbContextBase>, EntityService<TenantDbContextBase>>();
            services.AddScoped<IEntityService<ClientDBContextBase>, EntityService<ClientDBContextBase>>();
            services.AddTransient<IQueryableConnector<TenantDbContextBase>, DbConnector<TenantDbContextBase>>();
            services.AddTransient<IQueryableConnector<ClientDBContextBase>, DbConnector<ClientDBContextBase>>();
        }
    }
}

