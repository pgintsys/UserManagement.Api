using Emte.UserManagement.Models.Request;

namespace Emte.UserManagement.BusinessLogic
{
    public interface IRoleService
    {
        Task CreateRoles(CreateRoleRequest[] roleRequests, CancellationToken cancellationToken);
        Task MapUsersToRole(CreateUserRoleMapByEmail[] userRoleMap, CancellationToken cancellationToken);
        Task MapRoleToAccess(CreateRoleAccessMap[] roleAccessMap, CancellationToken cancellationToken);
    }
}