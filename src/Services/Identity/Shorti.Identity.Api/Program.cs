using Microsoft.EntityFrameworkCore;
using Shorti.Identity.Api.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShortiIdentityContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'ShortiIdentityContextConnection' not found.");

builder.Services.AddDbContext<ShortiIdentityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<ShortiIdentityContext>();

builder.Services.AddControllers();

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
