using System;
using Emte.Core.API;
using Emte.Core.Authentication.Contract;
using Emte.Core.JWTAuth;

namespace Emte.UserManagement.API.Configuration
{
	public class AppConfig : ISwaggerConfig
	{
        public bool MultiTenancyEnabled { get; set; }

        public Connection? ConnectionStrings { get; set; }

        public int SwaggerResponseCacheAgeSeconds { get; set; }

        public IList<SwaggerCustomHeader>? SwaggerCustomHeaders { get; set; }

        public AuthenticationConfiguration? Authentication { get; set; }
        public AdminAuthenticationConfiguration? AdminAuthentication { get; set; }
        public AppEmailConfig? AppEmailConfig { get; set; }
    }

    public class Connection
    {
        public string? DefaultConnection { get; set; }

        public string? ClientConnection { get; set; }
    }

    public class AuthenticationConfiguration : IAuthConfig
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? RefreshTokenSecretKey { get; set; }
        public string? AccessTokenSecretKey { get; set; }
        public double RefreshTokenExpiration { get; set; }
        public double AccessTokenExpiration { get; set; }
    }

    public class AdminAuthenticationConfiguration : IAdminAuthConfig
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? RefreshTokenSecretKey { get; set; }
        public string? AccessTokenSecretKey { get; set; }
        public double RefreshTokenExpiration { get; set; }
        public double AccessTokenExpiration { get; set; }
    }


    public class AppEmailConfig : IAppEmailConfig
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

