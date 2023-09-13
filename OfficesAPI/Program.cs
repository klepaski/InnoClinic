using Microsoft.Extensions.Configuration;
using OfficesAPI.Services;
using OfficesAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// For Entity Framework
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<OfficesDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IOfficeService, OfficeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
