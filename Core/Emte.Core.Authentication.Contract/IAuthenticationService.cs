using System;
namespace Emte.Core.Authentication.Contract
{
    public interface IAuthenticationService<T, T1>
    {
        Task<T> RegisterUser(IRegisterRequest registerRequest, CancellationToken cancellationToken);
        Task<IAuthenticationResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken);
        Task<T> ResetPassword(IRegisterRequest request, CancellationToken cancellationToken);
    }
}

