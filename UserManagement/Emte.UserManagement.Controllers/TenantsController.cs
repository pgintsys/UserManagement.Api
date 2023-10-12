
using System.Xml.Linq;
using Emte.UserManagement.BusinessLogic.Contracts;
using Emte.UserManagement.Models.Request;
using Emte.UserManagement.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Emte.UserManagement.Core;
using Emte.Core.API.Extenstions;

namespace Emte.UserManagement.Controllers;

[Route("api/[controller]/v1")]
[ApiController]
public class TenantsController : Controller
{
    private readonly ITenantService _tenantService;

    public TenantsController(
        ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpPost("Subscribe")]
    public async Task<CreateTenantResponse> Subscribe([FromBody] CreateTenantRequest tenantRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _tenantService.Subscribe(tenantRequest, cancellationToken);
    }

    [Authorize]
    [HttpPost("ApproveTenant")]
    [TenantHeaderFilter(Name = "tenantID", Required = true)]
    public async Task<IActionResult> ApproveTenant([FromBody] ApproveTenantRequest tenantRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        bool isSuperAdmin = HttpContext.IsSuperAdmin();
        if (!isSuperAdmin) { return Unauthorized(); }
        return Ok(await _tenantService.ApproveTenant(tenantRequest.TenantId, cancellationToken));
    }

    [Authorize]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllTenants(CancellationToken cancellationToken)
    {
        return Ok(await _tenantService.GetAllTenants(cancellationToken));
    }

    [Authorize]
    [HttpPut("Migrate")]
    [TenantHeaderFilter(Name = "tenantID", Required = true)]
    public async Task<IActionResult> Migrate(CancellationToken cancellationToken)
    {
        await _tenantService.Migrate(cancellationToken);
        return Ok();
    }
}

