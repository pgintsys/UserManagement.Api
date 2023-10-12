using System;
using Emte.Core.Authentication.Contract;

namespace Emte.Core.JWTAuth
{
	public class AccessTokenGenerator : TokenGeneratorBase, IAccessTokenGenerator
	{
        public string GenerateToken(string secretKey, string issuer, string audience, double expiration, Dictionary<string, string> claims)
        {
            return GenerateCoreToken(secretKey, issuer, audience, expiration, claims);
        }
        public string GenerateToken(string secretKey, string issuer, string audience, double expiration)
        {
            return GenerateCoreToken(secretKey, issuer, audience, expiration);
        }
    }
}

