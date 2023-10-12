using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Emte.Core.Authentication.Contract;
using Microsoft.IdentityModel.Tokens;

namespace Emte.Core.JWTAuth
{
    public class JWTTokenValidator<T> : ITokenValidator<T>
        where T : IAuthConfig
    {
        private readonly T jWTConfiguration;

        public JWTTokenValidator(T jWTConfiguration)
        {
            this.jWTConfiguration = jWTConfiguration;
        }

        public bool ValidateRefreshToken(string token)
        {
            try
            {
                Console.WriteLine($"{jWTConfiguration.RefreshTokenSecretKey} and token is {token}");
                Console.WriteLine("inga vardhu");
                var param = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.RefreshTokenSecretKey!)),
                    ValidIssuer = jWTConfiguration.Issuer,
                    ValidAudience = jWTConfiguration.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
                JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
                _handler.ValidateToken(token, param, out SecurityToken securityToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                Console.WriteLine($"{jWTConfiguration.AccessTokenSecretKey}");
                Console.WriteLine("inga vardhu");
                var param = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.AccessTokenSecretKey!)),
                    ValidIssuer = jWTConfiguration.Issuer,
                    ValidAudience = jWTConfiguration.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
                JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
                _handler.ValidateToken(token, param, out SecurityToken securityToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

