namespace Emte.UserManagement.Models.Request
{
    public class CreateUserRoleMapByEmail
    {
        public string? Email { get; set; }
        public string? RoleName { get; set; }
    }
}