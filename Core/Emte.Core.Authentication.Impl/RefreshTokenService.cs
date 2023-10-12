using System.Security.Claims;
using Emte.Core.API;
using Emte.Core.Authentication.Contract;
using Emte.Core.DomainModels;
using Emte.Core.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using IDomain = Emte.Core.DomainModels.IDomain;

namespace Emte.Core.Authentication.Impl
{
    public class RefreshTokenServiceBase<T, T1, T2>
        where T : class, IDomain, IWithId, ITokenDomain<T1>, new()
        where T1 : class, IDomain, IUserDomain
        where T2 : IAuthConfig
    {
        private readonly IRepository<T> _tokenMapRepository;
        private readonly ITokenValidator<T2> _tokenValidator;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IAuthConfig _authConfig;
        public RefreshTokenServiceBase(
            IRepository<T> tokenMapRepository,
            ITokenValidator<T2> tokenValidator,
            IAuthConfig authConfig,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator)
        {
            _tokenValidator = tokenValidator;
            _tokenMapRepository = tokenMapRepository;
            _refreshTokenGenerator = refreshTokenGenerator;
            _accessTokenGenerator = accessTokenGenerator;
            _tokenValidator = tokenValidator;
            _authConfig = authConfig;
        }

        public async Task ClearTokenMap(Guid userId, CancellationToken cancellationToken)
        {
            var tokenMap = (await _tokenMapRepository.Set.FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken))?.Id;
            await _tokenMapRepository.DeleteByIds<T>(new[] { tokenMap!.Value }, cancellationToken);
        }
        public async Task CreateTokenMap(T mapObj, CancellationToken cancellationToken)
        {
            await _tokenMapRepository.CreateAsync(mapObj, cancellationToken);
        }

        public async Task<IAuthenticationResponse> Refresh(string refreshToken, CancellationToken cancellationToken)
        {
            bool isValid = _tokenValidator.ValidateRefreshToken(refreshToken);
            if (!isValid) { return new AuthenticationResponse("Invalid Parameters"); }

            var map = await _tokenMapRepository.Set.Include(t => t.AppUser).FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken);
            if (map == null) { return new AuthenticationResponse("Invalid request"); }

            var authResponse = Refresh(map.AppUser!);
            map.Token = authResponse.RefreshToken;
            _tokenMapRepository.Update(map);

            return authResponse;
        }
        protected virtual void ConstructClaims(Dictionary<string, string> claimsDict, T1 user)
        {
            claimsDict.Add("Id", user.Id.ToString());
            claimsDict.Add(ClaimTypes.Email, user.Email!);
        }
        public IAuthenticationResponse Refresh(T1 user)
        {
            Console.WriteLine("Admin Auth");
            var claimsDict = new Dictionary<string, string>();
            ConstructClaims(claimsDict, user);
            string accessToken = _accessTokenGenerator.GenerateToken(_authConfig.AccessTokenSecretKey!, _authConfig.Issuer!, _authConfig.Audience!, _authConfig.AccessTokenExpiration, claimsDict);
            string refreshToken = _refreshTokenGenerator.GenerateToken(_authConfig.RefreshTokenSecretKey!, _authConfig.Issuer!, _authConfig.Audience!, _authConfig.RefreshTokenExpiration);


            return new AuthenticationResponse(accessToken, refreshToken);
        }
    }
    public class RefreshTokenService<T, T1> : RefreshTokenServiceBase<T, T1, IAuthConfig>, IRefreshTokenService<T, T1>
        where T : class, IDomain, ITokenDomain<T1>, new()
        where T1 : class, IDomain, IUserDomain
    {

        public RefreshTokenService(
            IRepository<T> tokenMapRepository,
            ITokenValidator<IAuthConfig> tokenValidator,
            IAuthConfig authConfig,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator) : base(tokenMapRepository, tokenValidator, authConfig, accessTokenGenerator, refreshTokenGenerator) { }
    }

    public class AdminRefreshTokenService<T, T1> : RefreshTokenServiceBase<T, T1, IAdminAuthConfig>, IRefreshTokenService<T, T1>
        where T : class, IDomain, ITokenDomain<T1>, new()
        where T1 : class, IDomain, IUserDomain
    {

        public AdminRefreshTokenService(
            IRepository<T> tokenMapRepository,
            ITokenValidator<IAdminAuthConfig> tokenValidator,
            IAdminAuthConfig authConfig,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator) : base(tokenMapRepository, tokenValidator, authConfig, accessTokenGenerator, refreshTokenGenerator) { }

        protected override void ConstructClaims(Dictionary<string, string> claimsDict, T1 user)
        {
            base.ConstructClaims(claimsDict, user);
            claimsDict.Add("Role", Contants.Roles.SuperAdmin);
        }
    }
}

