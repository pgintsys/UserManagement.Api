namespace Emte.Core.Authentication.Contract;
public interface ITokenGenerator
{
    string GenerateToken(
        string secretKey,
        string issuer,
        string audience,
        double expiration,
        Dictionary<string, string> claims);
    string GenerateToken(
    string secretKey,
    string issuer,
    string audience,
    double expiration);
}

public interface IAccessTokenGenerator : ITokenGenerator { }
public interface IRefreshTokenGenerator : ITokenGenerator { }

