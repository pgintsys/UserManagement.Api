using System;
namespace Emte.Core.Authentication.Contract
{
    public interface IResetPasswordService<T, T1>
    {
        Task InitiateResetPassword(string email, CancellationToken cancellationToken);
        Task<bool> CanResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken);
        Task ResetPassword(IResetPasswordRequest request, CancellationToken cancellationToken);
    }
}

