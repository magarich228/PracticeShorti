using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorti.Identity.Api;
using Shorti.Shared.Kernel.KernelExtensions;
using Microsoft.IdentityModel.Tokens;
using Shorti.Identity.Api.Data;
using Shorti.Identity.Api.Identity;
using Shorti.Identity.Api.Identity.Abstractions;
using Shorti.Identity.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShortiIdentityContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'ShortiIdentityContextConnection' not found.");

builder.Services.AddKernelServices(builder.Configuration);

var identityConfig = builder.Configuration.GetSection("IdentityConfiguration").Get<IdentityConfiguration>() ??
    throw new InvalidOperationException("Нет конфигурации аутентификации jwt.");
builder.Services.AddSingleton(identityConfig);

builder.Services.AddTransient<HashService>();

builder.Services.AddDbContext<ShortiIdentityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme,
    configure =>
    {
        configure.SaveToken = true;

        configure.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = identityConfig.Issuer,
            ValidAudience = identityConfig.Audience,
            RequireExpirationTime = true,
            RequireAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityConfig.ServiceKey)),
        };
    });

builder.Services.AddSingleton<IJwtIdentityManager, JwtIdentityManager>();
builder.Services.AddHostedService<JwtRefreshTokenCache>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();