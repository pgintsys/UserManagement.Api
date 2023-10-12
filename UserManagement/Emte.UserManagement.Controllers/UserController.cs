using System;
using AutoMapper;
using Emte.Core.Authentication.Contract;
using Emte.Core.Authentication.Impl;
using Emte.Core.DataAccess;
using Emte.UserManagement.Core;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;
using Emte.UserManagement.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emte.UserManagement.Controllers
{
    [Route("api/[controller]/v1")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IAuthenticationService<AppUser, AppUserTokenMap> _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly IRefreshTokenService<AppUserTokenMap, AppUser> _refreshTokenService;
        private readonly IResetPasswordService<AppUserResetTokenMap, AppUser> _resetPasswordService;
        private readonly IEntityService<ClientDBContextBase> _entityService;
        private readonly IMapper _mapper;

        public UserController(
            IAuthenticationService<AppUser, AppUserTokenMap> authenticationService,
            IRefreshTokenService<AppUserTokenMap, AppUser> refreshTokenService,
            IEntityService<ClientDBContextBase> entityService,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IResetPasswordService<AppUserResetTokenMap, AppUser> resetPasswordService)
        {
            _mapper = mapper;
            _entityService = entityService;
            _refreshTokenService = refreshTokenService;
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
            _resetPasswordService = resetPasswordService;
        }

        [HttpPost("Login")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IAuthenticationResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var resposen = await _authenticationService.Login(loginRequest, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return resposen;
        }

        [HttpPost("Refresh")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IAuthenticationResponse> Refresh(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            var response = await _refreshTokenService.Refresh(refreshTokenRequest.RefreshToken!, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return response;
        }

        [HttpPost("InitiateResetPassword")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task InitiateResetPassword(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            await _resetPasswordService.InitiateResetPassword(resetPasswordRequest.Email!, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
        }

        [HttpPost("ResetPassword")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task ResetPassword(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            await _resetPasswordService.ResetPassword(resetPasswordRequest, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
        }

        [HttpPost("Logout")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IAuthenticationResponse> Logout(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var resposen = await _authenticationService.Login(loginRequest, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return resposen;
        }

        [HttpPost("IsAuthorized")]
        [Authorize]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IAuthorizationResponse> IsAuthorized(AuthorizationRequest authorizationRequest, CancellationToken cancellationToken)
        {
            var strUserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            Guid userId;
            if (string.IsNullOrWhiteSpace(strUserId) || !Guid.TryParse(strUserId, out userId)) { return new AuthorizationResponse(false); }

            bool isAuthorized = await _authorizationService.IsAuthorized(authorizationRequest.AccessRequest!, userId, cancellationToken);
            return new AuthorizationResponse(isAuthorized);
        }
    }
}

