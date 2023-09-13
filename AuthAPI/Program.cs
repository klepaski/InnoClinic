//using AuthAPI.Consumeres;
using AuthAPI.Models;
using AuthAPI.Services;
//using HotelingLibrary;
//using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string secret = builder.Configuration.GetValue<string>("JwtSecret");

//token
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true
};
builder.Services.AddSingleton(tokenValidationParameters);

//ef
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddControllers();
//services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//rabbitmq
//builder.Services.AddMassTransit(config => {
//    config.AddConsumer<UserChangedConsumer>();

//    config.UsingRabbitMq((ctx, cfg) => {
//        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
//        cfg.ReceiveEndpoint(QueuesUrls.User_ReviewDeleted, c => {
//            c.ConfigureConsumer<UserChangedConsumer>(ctx);
//        });

//    });
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
