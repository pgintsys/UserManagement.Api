using AutoMapper;
using Emte.Core.Authentication.Contract;
using Emte.UserManagement.DomainModels;
using Emte.UserManagement.Models.Request;
using Emte.UserManagement.Models.Response;
using System.Security.Cryptography;
namespace Emte.UserManagement.BusinessLogic
{
    public class UserManagementProfiles : Profile
    {
        public UserManagementProfiles()
        {
            CreateMap<CreateTenantRequest, Tenant>()
                .ForMember(t => t.StatusId, opts => opts.Ignore())
                .ForMember(t => t.Status, opts => opts.Ignore());
            CreateMap<Tenant, TenantDataResponse>();
            CreateMap<Tenant, CreateTenantResponse>();
            CreateMap<CreateUserRequest, AppUser>()
                .ForMember(t => t.Id, opts => opts.Ignore())
                .ForMember(t => t.FirstName, opts => opts.MapFrom(o => o.FirstName))
                .ForMember(t => t.LastName, opts => opts.MapFrom(o => o.LastName))
                .ForMember(t => t.Email, opts => opts.MapFrom(o => o.Email));

            CreateMap<Tenant, CreateUserRequest>()
                .ForMember(u => u.FirstName, opts => opts.MapFrom(o => o.Name))
                .ForMember(u => u.LastName, opts => opts.MapFrom(o => o.Name))
                .ForMember(u => u.Email, opts => opts.MapFrom(o => o.Email))
                .ForMember(u => u.Password, opts => opts.MapFrom(o => Guid.NewGuid().ToString("d").Substring(1, 8)))
                .ForMember(t => t.ConfirmPassword, opts => opts.Ignore());

            CreateMap<AppUser, AppUserTokenMap>()
                .ForMember(m => m.UserId, opts => opts.MapFrom(o => o.Id))
                .ForMember(m => m.AppUser, opts => opts.Ignore())
                .ForMember(m => m.Token, opts => opts.Ignore());

            CreateMap<CreateRoleRequest, AppRole>()
                .ForMember(m => m.Id, opts => opts.Ignore())
                .ForMember(m => m.Users, opts => opts.Ignore())
                .ForMember(m => m.Name, opts => opts.MapFrom(o => o.Name));

            CreateMap<CreateAccessRequest, AppAccess>()
                .ForMember(m => m.Id, opts => opts.Ignore())
                .ForMember(m => m.Name, opts => opts.MapFrom(o => o.Name));

            CreateMap<AppUser, AppUserResetTokenMap>()
                .ForMember(a => a.AppUser, opts => opts.Ignore())
                .ForMember(a => a.UserId, opts => opts.MapFrom(r => r.Id))
                .ForMember(a => a.Token, opts => opts.MapFrom(r => GenerateToken()));

            CreateMap<AppUser, AppUser>();
        }

        private string GenerateToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}

