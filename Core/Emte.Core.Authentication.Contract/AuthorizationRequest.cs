using System.ComponentModel.DataAnnotations;

namespace Emte.Core.Authentication.Contract
{
    public class AuthorizationRequest
    {
        [Required]
        public string? AccessRequest { get; set; }
    }
}