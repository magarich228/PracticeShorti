using Microsoft.EntityFrameworkCore;
using Shorti.Activities.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ActivitiesContext") ??
    throw new InvalidOperationException("Connection string 'ActivitiesContext' not found.");

builder.Services.AddDbContext<ActivitiesContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
