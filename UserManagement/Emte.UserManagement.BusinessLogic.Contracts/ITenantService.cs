﻿using Emte.UserManagement.DomainModels;
using Emte.UserManagement.Models;
using Emte.UserManagement.Models.Request;
using Emte.UserManagement.Models.Response;

namespace Emte.UserManagement.BusinessLogic.Contracts;
public interface ITenantService
{
    Task<CreateTenantResponse> Subscribe(CreateTenantRequest model, CancellationToken cancellationToken);
    Task<ApproveTenantResponse> ApproveTenant(Guid tenantId, CancellationToken cancellationToken);
    Task<TenantDataResponse[]> GetAllTenants(CancellationToken cancellationToken);
    Task Migrate(CancellationToken cancellationToken);
}

