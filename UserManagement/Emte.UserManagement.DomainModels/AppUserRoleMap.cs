using System.ComponentModel.DataAnnotations.Schema;
using Emte.Core.DomainModels;
using Emte.UserManagement.DomainModels;

namespace Emte.UserManagement.DomainModels
{
    public class AppUserRoleMap : IDomain, IWithId
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AppUser? AppUser { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual AppRole? AppRole { get; set; }
    }
}
