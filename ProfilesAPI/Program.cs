using ProfilesAPI.Services;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Context;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<ProfilesDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


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
