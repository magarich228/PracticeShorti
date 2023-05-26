using Microsoft.EntityFrameworkCore;
using Shorti.Shared.Contracts.Services;
using Shorti.Shared.Kernel.KernelExtensions;
using Shorti.ShortsService.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ShortsContext") ??
    throw new InvalidOperationException("Connection string 'ShortsContext' not found."); ;

builder.Services.AddDbContext<ShortsContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddContractsServicesClients();
builder.Services.AddKernelServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin());

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
