using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using wedev.Service.Services.Global;
using wedev.Service.Services;
using wedev.Infrastructure;
using wedev.Auth.JWT;
using wedev.Auth.Interfaces;
using wedev.Auth.Services;
using wedev.Auth.Common;
using wedev.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Hinzufügen von Konfigurationen und Dienstregistrierungen
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddDbContextFactory<GlobalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GlobalDatabase")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddScoped<PasswordHashing>();

// Standarddienste hinzufügen
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// JWT-Middleware korrekt in die Pipeline einfügen
//app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
