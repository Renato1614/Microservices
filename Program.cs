using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Play.Catalog.Service.Models;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
ServiceSettings _servicesettings;

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

builder.Services.AddMongo()
                .AddMongoRepósitory<Item>("Item");




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
