using System;
namespace Emte.Core.Authentication.Contract
{
    public class ResetPasswordRequest : IResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? ResetToken { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}

