using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shorti.Shared.Kernel.Identity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Configurations/identity.json", false, true);
builder.Configuration.AddJsonFile("Configurations/shorts.json", false, true);

var identityConfig = new IdentityConfiguration();
builder.Services.AddSingleton(identityConfig);

var authProviderKey = "ShortiIdentity";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
    authProviderKey,
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

builder.Services.AddAuthorization();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseOcelot();

app.Run();
