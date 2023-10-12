﻿using System;
using Emte.UserManagement.API.DataAccess;
using Emte.UserManagement.BusinessLogic;
using Emte.UserManagement.Core;
using Emte.UserManagement.Models;
using Microsoft.EntityFrameworkCore;
using Constants = Emte.UserManagement.BusinessLogic.Constants;

namespace Emte.UserManagement.API.Middlewares
{
    public class TenantIdentifier
    {
        private readonly RequestDelegate _next;

        public TenantIdentifier(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, TenantDbContext dbContext)
        {
            Guid tenantId = Guid.Empty;
            Console.WriteLine($"tenantID - {tenantId}");
            // from claims
            if (httpContext.Items.TryGetValue(Constants.BaseWebApi.EMTEClaims, out var eMTEClaimsObject))
            {
                tenantId = (eMTEClaimsObject as EMTEClaims)?.TenantId ?? Guid.Empty;
                Console.WriteLine($"tenantID - {tenantId}");
            }
            // from header
            else if (tenantId == Guid.Empty && httpContext.Request.Headers.TryGetValue(Constants.BaseWebApi.TenantHeader, out var tenantIdInHeader))
            {
                Console.WriteLine($"tenantIDStr - {tenantIdInHeader}");
                if (!string.IsNullOrEmpty(tenantIdInHeader))
                {
                    tenantId = Guid.Parse(tenantIdInHeader!);
                }
                Console.WriteLine($"tenantID - {tenantId}");
            }

            if (tenantId != Guid.Empty)
            {
                Console.WriteLine($"tenantID - {tenantId}");
                var tenant = dbContext.Tenants?.AsNoTracking().FirstOrDefault(t => t.Id == tenantId);
                var tenantInfo = new TenantModel
                {
                    Id = tenant!.Id,
                    Name = tenant!.Name,
                    Email = tenant!.Email
                };

                httpContext.Items[Constants.Common.Tenant] = tenantInfo;
                Console.WriteLine($"tenantInfo - {Newtonsoft.Json.JsonConvert.SerializeObject(tenantInfo)}");
            }

            await _next.Invoke(httpContext);
        }
    }

    public static class TenantIdentifierExtension
    {
        public static IApplicationBuilder UseTenantIdentifier(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantIdentifier>();
            return app;
        }
    }
}

