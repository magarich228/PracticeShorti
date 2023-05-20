using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shorti.Identity.Api.Data;
using Shorti.Identity.Api.Identity;
using Shorti.Identity.Api.Identity.Abstractions;
using Shorti.Identity.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShortiIdentityContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'ShortiIdentityContextConnection' not found.");

var identityConfig = builder.Configuration.GetSection("IdentityConfiguration").Get<IdentityConfiguration>() ??
    throw new InvalidOperationException("��� ������������ �������������� jwt.");
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
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();;

app.UseAuthorization();

app.MapControllers();

app.Run();