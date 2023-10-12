using System.ComponentModel.DataAnnotations.Schema;
using Emte.Core.DomainModels;
using Emte.UserManagement.DomainModels;

namespace Emte.UserManagement.DomainModels
{
    public class AppRoleAccessMap : IDomain, IWithId
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid AccessId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual AppRole? AppRole { get; set; }
        [ForeignKey(nameof(AccessId))]
        public virtual AppAccess? Access { get; set; }
    }
}
