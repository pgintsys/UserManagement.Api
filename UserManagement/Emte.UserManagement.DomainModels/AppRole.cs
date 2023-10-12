using Emte.Core.DomainModels;

namespace Emte.UserManagement.DomainModels
{
    public class AppRole : IRoleDomain
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<AppUserRoleMap>? Users { get; set; }
    }
}