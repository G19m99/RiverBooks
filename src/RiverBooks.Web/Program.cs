using FastEndpoints;
using FastEndpoints.Security;
using RiverBooks.Books;
using Serilog;
using System.Reflection;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
    config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddOpenApi();

//Module services
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
builder.Services.AddBooksServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddUsersModuleServices(builder.Configuration, logger, mediatRAssemblies);


//Set up MediatoR - with ability foreach module to select whether it wants to opt-in
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));


string jwtSecret = builder.Configuration["Auth:JwtSecret"]!;

builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = jwtSecret)
    .AddAuthorization()
    .AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints();

app.Run();

//class is partial to support testing
public partial class Program { }

//Migartion instructions for reg and testing dbs

//standard

//testing
//1: dotnet ef database update -- --enviroment Testing