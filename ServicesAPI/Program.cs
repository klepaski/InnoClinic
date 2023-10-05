using ServicesAPI.Services;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Context;
using MassTransit;
using MassTransit.MultiBus;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<ServicesDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//rabbit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient<IServiceService, ServiceService>();
builder.Services.AddTransient<ISpecializationService, SpecializationService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
