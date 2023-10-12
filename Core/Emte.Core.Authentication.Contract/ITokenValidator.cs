using System;
namespace Emte.Core.Authentication.Contract
{
	public interface ITokenValidator<T>
		where T : IAuthConfig
	{
		bool ValidateToken(string token);
		bool ValidateRefreshToken(string token);
	}
}

