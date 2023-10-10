using ProfilesAPI.Services;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Context;
using MassTransit;
using ProfilesAPI.Consumers;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<ProfilesDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//rabbit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SpecializationCreatedConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri("rabbitmq://localhost/"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("specialization-created-event", e =>
        {
            e.ConfigureConsumer<SpecializationCreatedConsumer>(provider);
        });
    }));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient<IDoctorService, DoctorService>();
builder.Services.AddTransient<IPatientService, PatientService>();
builder.Services.AddTransient<IReceptionistService, ReceptionistService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSingleton<IOfficeService, OfficeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
