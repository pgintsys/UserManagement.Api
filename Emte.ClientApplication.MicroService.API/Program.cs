using Emte.ClientApplication.MicroService.API;
using Emte.Core.Authentication.Contract;
using Emte.Core.JWTAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile(@"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($@"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

var Configuration = configurationBuilder.Build();
// Add services to the container.
builder.Services.Configure<AppConfig>(Configuration);

var appConfig = new AppConfig();
Configuration.Bind(appConfig);
builder.Services.AddTransient<IAuthConfig>((p) => appConfig.Authentication!);
builder.Services.AddTransient<IAppEmailConfig>((p) => appConfig.AppEmailConfig!);
builder.Services.InitializeJWTAuthentication(appConfig.Authentication!);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    //Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

var app = builder.Build();
bool isLocalEnvironment = builder.Environment.EnvironmentName.Equals("Local");
Console.WriteLine($"Environment - {builder.Environment.EnvironmentName}");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || isLocalEnvironment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!isLocalEnvironment) { app.UseHttpsRedirection(); }

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

