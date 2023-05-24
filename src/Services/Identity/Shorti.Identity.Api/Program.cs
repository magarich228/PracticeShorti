using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorti.Shared.Kernel.KernelExtensions;
using Shorti.Identity.Api.Data;
using Shorti.Identity.Api.Identity;
using Shorti.Identity.Api.Identity.Abstractions;
using Shorti.Identity.Api.Services;
using Shorti.Shared.Kernel.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShortiIdentityContextConnection") ??
    throw new InvalidOperationException("Connection string 'ShortiIdentityContextConnection' not found.");

builder.Services.AddKernelServices(builder.Configuration);

var identityConfig = new IdentityConfiguration();
builder.Services.AddSingleton(identityConfig);

builder.Services.AddTransient<HashService>();

builder.Services.AddDbContext<ShortiIdentityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

builder.Services.AddSingleton<IJwtIdentityManager, JwtIdentityManager>();
builder.Services.AddHostedService<JwtRefreshTokenCache>();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication",
        Description = "Shorti Identity Service API"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin());

app.UseStaticFiles();

app.MapControllers();

app.Run();