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
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IEntityService<ClientDBContextBase> _entityService;

        public RolesController(
            IRoleService roleService,
            IAuthorizationService authorizationService,
            IEntityService<ClientDBContextBase> entityService)
        {
            _roleService = roleService;
            _entityService = entityService;
            _authorizationService = authorizationService;
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
        [HttpPost("AddCustomRoles")]
        [TenantHeaderFilter(Name = "tenantId", Required = true)]
        public async Task<IActionResult> CreateCustomRoles(CreateRoleRequest[] roleRequests, CancellationToken cancellationToken)
        {
            await _roleService.CreateRoles(roleRequests, cancellationToken);
            await _entityService.SaveAsync(cancellationToken);
            return Ok();
        }
    }
}

