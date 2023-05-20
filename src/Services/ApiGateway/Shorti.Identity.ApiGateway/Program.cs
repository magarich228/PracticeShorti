using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Configurations/identity.json");
builder.Configuration.AddJsonFile("Configurations/shorts.json");

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseOcelot();

app.Run();
