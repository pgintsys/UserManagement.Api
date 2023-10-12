using Emte.UserManagement.API.Configuration;
using Emte.UserManagement.API.Extensions;
using Emte.UserManagement.API.Middlewares;
using Emte.Core.JWTAuth;
using Emte.Core.Authentication.Contract;
using Microsoft.OpenApi.Models;
using Emte.UserManagement.Core;
using Emte.UserManagement.API.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile(@"appsettings.Local.json", optional: true, reloadOnChange: true)
                .AddJsonFile($@"appsettings.Local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

var Configuration = configurationBuilder.Build();
// Add services to the container.
builder.Services.Configure<AppConfig>(Configuration);

var appConfig = new AppConfig();
Configuration.Bind(appConfig);
builder.Services.AddTransient<IAuthConfig>((p) => appConfig.Authentication!);
builder.Services.AddTransient<IAdminAuthConfig>((p) => appConfig.AdminAuthentication!);
builder.Services.AddTransient<IAppEmailConfig>((p) => appConfig.AppEmailConfig!);
builder.Services.InitializeJWTAdminAuthentication(appConfig.Authentication!, appConfig.AdminAuthentication!);

builder.Services.AddControllers();
builder.Services.RegisterServiceCollection(Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setup =>
{
    setup.OperationFilter<TenantHeaderFilter>();
    //Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Admin Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = "AdminBearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    var jwtBearerSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Bearer Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityDefinition(jwtBearerSecurityScheme.Reference.Id, jwtBearerSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() },
        { jwtBearerSecurityScheme, Array.Empty<string>() }
    });

});

builder.Services.AddHealthChecks();


var app = builder.Build();
app.MapHealthChecks("/healthcheck");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
bool isLocalEnvironment = builder.Environment.EnvironmentName.Equals("Local");
Console.WriteLine($"Environment - {builder.Environment.EnvironmentName}");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || isLocalEnvironment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!isLocalEnvironment) { app.UseHttpsRedirection(); }

app.UseTenantIdentifier();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

