using System.Text;
using Emte.Core.Authentication.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Emte.Core.JWTAuth;
public static class JWTRegistrar
{

    public static void InitializeJWTAuthentication(this IServiceCollection services, IAuthConfig jWTConfiguration)
    {
        services.AddTransient<ITokenValidator<IAuthConfig>, JWTTokenValidator<IAuthConfig>>();
        services.AddTransient<IAccessTokenGenerator, AccessTokenGenerator>();
        services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
        {
            o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.AccessTokenSecretKey!)),
                ValidIssuer = jWTConfiguration.Issuer,
                ValidAudience = jWTConfiguration.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }


    public static void InitializeJWTAdminAuthen(this IServiceCollection services, IAdminAuthConfig jWTConfiguration)
    {
        services.AddTransient<ITokenValidator<IAdminAuthConfig>, JWTTokenValidator<IAdminAuthConfig>>();
        services.AddTransient<ITokenValidator<IAuthConfig>, JWTTokenValidator<IAuthConfig>>();
        services.AddTransient<IAccessTokenGenerator, AccessTokenGenerator>();
        services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
        {
            o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.AccessTokenSecretKey!)),
                ValidIssuer = jWTConfiguration.Issuer,
                ValidAudience = jWTConfiguration.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
    public static void InitializeJWTAdminAuthentication(this IServiceCollection services, IAuthConfig authConfig, IAdminAuthConfig jWTConfiguration)
    {
        services.AddTransient<ITokenValidator<IAdminAuthConfig>, JWTTokenValidator<IAdminAuthConfig>>();
        services.AddTransient<ITokenValidator<IAuthConfig>, JWTTokenValidator<IAuthConfig>>();
        services.AddTransient<IAccessTokenGenerator, AccessTokenGenerator>();
        services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
        {
            o.ForwardDefaultSelector = ctx => "AdminBearer";
            o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessTokenSecretKey!)),
                ValidIssuer = authConfig.Issuer,
                ValidAudience = authConfig.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        })
        .AddJwtBearer("AdminBearer", o =>
        {

            o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.AccessTokenSecretKey!)),
                ValidIssuer = jWTConfiguration.Issuer,
                ValidAudience = jWTConfiguration.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}

