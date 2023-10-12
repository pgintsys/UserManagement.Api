using System.Security.Claims;
using AutoMapper;
using Emte.Core.API;
using Emte.Core.Authentication.Contract;
using Emte.Core.DomainModels;
using Emte.Core.Repository.Contracts;

namespace Emte.Core.Authentication.Impl;
public class AuthenticationService<T1, T2> : IAuthenticationService<T1, T2>
    where T1 : class, IDomain, IUserDomain
    where T2 : class, IDomain, ITokenDomain<T1>

{
    private readonly ITokenValidator<IAdminAuthConfig> _adminTokenValidator;
    private readonly IRefreshTokenService<T2, T1> _refreshTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository<T1> _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IMapper mapper,
        IAuthConfig authConfig,
        ITokenValidator<IAuthConfig> tokenValidator,
        IPasswordHasher passwordHasher,
        IUserRepository<T1> userRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenService<T2, T1> refreshTokenService,
        ITokenValidator<IAdminAuthConfig> adminTokenValidator)
    {
        _refreshTokenGenerator = refreshTokenGenerator;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenService = refreshTokenService;
        _adminTokenValidator = adminTokenValidator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<IAuthenticationResponse> Login(LoginRequest registerRequest, CancellationToken cancellationToken)
    {
        // to-do validate
        var user = await _userRepository.GetUserByEmail(registerRequest.Email!, cancellationToken);
        if (user == null || !_passwordHasher.VerifyPassword(registerRequest.Password!, user.HashedPassword!)) { return new AuthenticationResponse("User UnAuthorized"); }

        string passwordHash = _passwordHasher.HashPassword(registerRequest.Password!);
        var response = _refreshTokenService.Refresh(user);

        var tokenMap = _mapper.Map<T2>(user);
        tokenMap.Token = response.RefreshToken;
        await _refreshTokenService.CreateTokenMap(tokenMap, cancellationToken);

        return response;
    }

    public async Task Logout(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.ReadByIdAsync(userId, cancellationToken);
        if (user == null) { throw new UnauthorizedAccessException("User not found"); }
        await _refreshTokenService.ClearTokenMap(userId, cancellationToken);
    }

    public async Task<T1> RegisterUser(IRegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        // to-do validate
        if (!registerRequest.Password!.Equals(registerRequest.ConfirmPassword)) { throw new Exception("Bad Request"); }
        var user = await _userRepository.GetUserByEmail(registerRequest.Email!, cancellationToken);
        if (user != null) { throw new Exception("user already available"); }

        string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

        var newUser = _mapper.Map<T1>(registerRequest);
        newUser.HashedPassword = passwordHash;

        var createdUser = await _userRepository.CreateUser(newUser, cancellationToken);
        return createdUser;
    }

    public async Task<T1> ResetPassword(IRegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        // to-do validate
        if (!registerRequest.Password!.Equals(registerRequest.ConfirmPassword)) { throw new Exception("Bad Request"); }
        var user = await _userRepository.GetUserByEmail(registerRequest.Email!, cancellationToken);
        if (user == null) { throw new Exception("user not available"); }

        string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
        user.HashedPassword = passwordHash;

        var createdUser = _userRepository.Update(user);
        return createdUser;
    }
}

