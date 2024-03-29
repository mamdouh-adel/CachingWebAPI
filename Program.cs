using System;
using CachingWebAPI.Data;
using CachingWebAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string dbConnectionString = builder.Configuration.GetConnectionString("Default");
string dbPassword = Environment.GetEnvironmentVariable("DbPassword");

dbConnectionString = dbConnectionString.Replace("{PASS}", dbPassword);

// Add ConnectionStrings
builder.Services.AddEntityFrameworkNpgsql()
       .AddDbContext<AppDbContext>(
        opt => opt.UseNpgsql(dbConnectionString)
       );

builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IDriversProvider, DriversWithCacheProvider>();
builder.Services.AddScoped<IDriversRepository, DriversRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
