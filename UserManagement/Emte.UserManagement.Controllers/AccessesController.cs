using System;
using Emte.Core.Authentication.Contract;
using System.Xml.Linq;
using Emte.UserManagement.BusinessLogic;
using Emte.UserManagement.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Emte.UserManagement.Models.Request;
using Emte.Core.DataAccess;
using Emte.UserManagement.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Emte.Core.API.Extenstions;

namespace Emte.UserManagement.Controllers
{

    [Route("api/[controller]/v1")]
    [ApiController]
    public class AccessesController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IAccessService _accessService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IEntityService<ClientDBContextBase> _entityService;

        public AccessesController(
            IRoleService roleService,
            IAuthorizationService authorizationService,
            IEntityService<ClientDBContextBase> entityService,
            IAccessService accessService)
        {
            _roleService = roleService;
            _entityService = entityService;
            _authorizationService = authorizationService;
            _accessService = accessService;
        }

        [Authorize]
        [HttpPost("AddRoles")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IActionResult> CreateRoles(CreateRoleRequest[] roleRequests, CancellationToken cancellationToken)
        {
            bool isSuperAdmin = HttpContext.IsSuperAdmin();
            if (!isSuperAdmin) { return Unauthorized(); }
            await _roleService.CreateRoles(roleRequests, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("AddAccesses")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IActionResult> CreateAccessess(CreateAccessRequest[] accessRequests, CancellationToken cancellationToken)
        {
            bool isSuperAdmin = HttpContext.IsSuperAdmin();
            if (!isSuperAdmin) { return Unauthorized(); }
            await _accessService.CreateAccesses(accessRequests, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("AddCustomAccesses")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IActionResult> CreateCustomAccessess(CreateAccessRequest[] accessRequests, CancellationToken cancellationToken)
        {
            await _accessService.CreateAccesses(accessRequests, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("AddCustomRoles")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IActionResult> CreateCustomRoles(CreateRoleRequest[] roleRequests, CancellationToken cancellationToken)
        {
            await _roleService.CreateRoles(roleRequests, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("AssociateUserRole")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IActionResult> CreateCustomRoles(CreateUserRoleMapByEmail[] roleRequests, CancellationToken cancellationToken)
        {
            await _roleService.MapUsersToRole(roleRequests, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("AssociateRoleAccess")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IActionResult> CreateCustomAccess(CreateRoleAccessMap[] roleRequests, CancellationToken cancellationToken)
        {
            await _roleService.MapRoleToAccess(roleRequests, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return Ok();
        }
    }
}

