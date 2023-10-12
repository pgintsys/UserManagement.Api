using System;
using System.ComponentModel.DataAnnotations;

namespace Emte.Core.Authentication.Contract
{
    public interface IResetPasswordRequest : IRegisterRequest
    {
        public string? ResetToken { get; set; }
    }
}

