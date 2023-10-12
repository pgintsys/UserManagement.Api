using AutoMapper;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DomainModels;
using Emte.UserManagement.Models.Request;

namespace Emte.UserManagement.BusinessLogic
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<AppRole> _roleRepository;
        private readonly IUserRepository<AppUser> _userRepository;
        private readonly IRepository<AppAccess> _accessRepository;
        private readonly IRepository<AppUserRoleMap> _userRoleMapRepository;
        private readonly IRepository<AppRoleAccessMap> _roleAccessMapRepository;
        public RoleService(
            IMapper mapper,
            IRepository<AppRole> roleRepository,
            IUserRepository<AppUser> userRepository,
            IRepository<AppUserRoleMap> userRoleMapRepository,
            IRepository<AppRoleAccessMap> roleAccessMapRepository,
            IRepository<AppAccess> accessRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleMapRepository = userRoleMapRepository;
            _roleAccessMapRepository = roleAccessMapRepository;
            _accessRepository = accessRepository;
        }

        public async Task CreateRoles(CreateRoleRequest[] roleRequests, CancellationToken cancellationToken)
        {
            var rolesToCreate = roleRequests.Select(r => _mapper.Map<AppRole>(r)).ToArray();
            Console.WriteLine($"Roles creating {string.Join(", ", rolesToCreate.Select(r => Newtonsoft.Json.JsonConvert.SerializeObject(r)))}");
            await _roleRepository.CreateMultipleAsync(rolesToCreate, cancellationToken);
        }

        public async Task MapUsersToRole(CreateUserRoleMapByEmail[] userRoleMap, CancellationToken cancellationToken)
        {
            var emails = userRoleMap.Select(u => u.Email).Distinct();
            var roles = userRoleMap.Select(u => u.RoleName).Distinct();

            var emailToUserIdMap = _userRepository.Set.Where(u => emails.Contains(u.Email)).Select(u => new { u.Id, u.Email }).ToDictionary(u => u.Email!, u => u.Id);
            var roleToRoleIdMap = _roleRepository.Set.Where(r => roles.Contains(r.Name)).Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Name!, r => r.Id);

            var appUserRoleMaps = userRoleMap.Select(u => new AppUserRoleMap
            {
                UserId = emailToUserIdMap[u.Email!],
                RoleId = roleToRoleIdMap[u.RoleName!]
            }).ToArray();

            await _userRoleMapRepository.CreateMultipleAsync(appUserRoleMaps, cancellationToken);
        }

        public async Task MapRoleToAccess(CreateRoleAccessMap[] roleAccessMap, CancellationToken cancellationToken)
        {
            var accessess = roleAccessMap.Select(u => u.Access).Distinct();
            var roles = roleAccessMap.Select(u => u.RoleName).Distinct();

            var accessNameToAccessMap = _accessRepository.Set.Where(u => accessess.Contains(u.Name)).Select(u => new { u.Id, u.Name }).ToDictionary(u => u.Name!, u => u.Id);
            var roleToRoleIdMap = _roleRepository.Set.Where(r => roles.Contains(r.Name)).Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Name!, r => r.Id);

            var appRoleAccessMaps = roleAccessMap.Select(u => new AppRoleAccessMap
            {
                AccessId = accessNameToAccessMap[u.Access!],
                RoleId = roleToRoleIdMap[u.RoleName!]
            }).ToArray();

            await _roleAccessMapRepository.CreateMultipleAsync(appRoleAccessMaps, cancellationToken);
        }
    }
}