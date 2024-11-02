using Microsoft.EntityFrameworkCore;
using wedev.Auth.Common;
using wedev.Auth.Interfaces;
using wedev.Auth.JWT;
using wedev.Auth.Services;
using wedev.Infrastructure;
using wedev.Service.Services;
using wedev.Service.Services.Global; // Vergewissern Sie sich, dass der Namespace für GlobalServices enthalten ist

var builder = WebApplication.CreateBuilder(args);

// Hinzufügen von Konfigurationen und Dienstregistrierungen
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddDbContextFactory<GlobalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GlobalDatabase")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddScoped<PasswordHashing>();
builder.Services.AddScoped<GlobalDbContext>();

// Registrierung von GlobalServices
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<GlobalServices>();
builder.Services.AddScoped<AppService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<RoleService>();
// Standarddienste hinzufügen
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();