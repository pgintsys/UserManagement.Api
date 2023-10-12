
using Emte.Core.API.Extenstions;
using Emte.Core.DomainModels;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DomainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Emte.UserManagement.BusinessLogic
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository<AppAccess> _accessRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<AppUserRoleMap> _userRoleMapRepository;
        private readonly IRepository<AppRoleAccessMap> _roleAccessMapRepository;
        public AuthorizationService(
            IRepository<AppRoleAccessMap> roleAccessMapRepository,
            IRepository<AppUserRoleMap> userRoleMapRepository,
            IRepository<AppAccess> accessRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _roleAccessMapRepository = roleAccessMapRepository;
            _userRoleMapRepository = userRoleMapRepository;
            _httpContextAccessor = httpContextAccessor;
            _accessRepository = accessRepository;
        }
        public async Task<bool> IsAuthorized(string access, Guid userId, CancellationToken cancellationToken)
        {
            bool isSuperAdmin = _httpContextAccessor.HttpContext!.IsSuperAdmin();
            if (isSuperAdmin) { return true; }
            var result = from roleAccessMap in _roleAccessMapRepository.Set
                         join ac in _accessRepository.Set on roleAccessMap.AccessId equals ac.Id
                         join userRoleMap in _userRoleMapRepository.Set on roleAccessMap.RoleId equals userRoleMap.RoleId
                         where userRoleMap.UserId == userId && ac.Name == access
                         select ac;

            return await result.Take(1).AnyAsync(cancellationToken);
        }
    }
}