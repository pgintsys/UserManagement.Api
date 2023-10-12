using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Emte.UserManagement.Core
{
    public class TenantHeaderFilter : Attribute, IOperationFilter
    {
        public string? Name { get; set; }
        public bool Required { get; set; }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var addHeaders = context.MethodInfo.GetCustomAttributes(typeof(TenantHeaderFilter), true).Cast<TenantHeaderFilter>();
            foreach (var header in addHeaders)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema { Type = "string" },
                    Name = header.Name,
                    Required = header.Required
                });
            }
        }
    }
}

