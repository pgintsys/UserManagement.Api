namespace Emte.UserManagement.Models.Response
{
    public class TenantDataResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MetaData { get; set; }
    }
}