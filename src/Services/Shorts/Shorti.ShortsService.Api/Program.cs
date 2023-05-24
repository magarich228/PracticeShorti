using Microsoft.EntityFrameworkCore;
using Shorti.Shared.Kernel.KernelExtensions;
using Shorti.ShortsService.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ShortsContext");

builder.Services.AddHttpClient("IdentityClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5171/");
});

builder.Services.AddDbContext<ShortsContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddKernelServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
