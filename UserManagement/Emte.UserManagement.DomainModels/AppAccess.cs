using Emte.Core.DomainModels;

namespace Emte.UserManagement.DomainModels
{
    public class AppAccess : IAccessDomain
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}