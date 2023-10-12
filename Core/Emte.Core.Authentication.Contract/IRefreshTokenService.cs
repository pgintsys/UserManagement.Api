using System;
namespace Emte.Core.Authentication.Contract
{
	public interface IRefreshTokenService<T, T1>
	{
		Task ClearTokenMap(Guid userId, CancellationToken cancellationToken);
		Task CreateTokenMap(T mapObj, CancellationToken cancellationToken);
		Task<IAuthenticationResponse> Refresh(string refreshToken, CancellationToken cancellationToken);
		IAuthenticationResponse Refresh(T1 user);
    }
}

