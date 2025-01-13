using FastEndpoints;
using RiverBooks.Books;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Module services
builder.Services.AddBooksServices(builder.Configuration);
builder.Services.AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseFastEndpoints();

app.Run();

//class is partial to support testing
public partial class Program { }

//Migartion instructions for reg and testing dbs

//standard

//testing
//1: dotnet ef database update -- --enviroment Testing