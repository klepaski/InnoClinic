using Microsoft.EntityFrameworkCore;
using AppointmentsAPI.Context;
using AppointmentsAPI.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<AppointmentDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//log
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

builder.Services.AddHttpClient();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<IResultService, ResultService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
