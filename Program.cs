using FastEndpoints;
using FluentMigrator.Runner;
using TemplateLinq2DbFastEndpoints;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// AddDatabaseServices is extension method in ConfigureServices.cs
builder.Services.AddDatabaseServices(connectionString);

builder.Services.AddFastEndpoints();

var app = builder.Build();

// run database migrations before app startup
using var scope = app.Services.CreateScope();
var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
migrationRunner.MigrateUp();

// Most basic "minimal api". https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api.
// See Endpoints folder for endpoints using the FastEndpoints library (also uses minimal api under the hood).
app.MapGet("/", () => "Hello World!");

app.UseFastEndpoints(config =>
{
    config.Endpoints.Configurator = ep =>
    {
        ep.AllowAnonymous();
    };
});

app.Run();
