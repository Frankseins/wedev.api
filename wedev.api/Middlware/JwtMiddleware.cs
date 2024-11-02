using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using wedev.Auth.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace wedev.WebApi.Middleware
{ 
public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        // Erstellen Sie einen neuen Scope für Scoped-Dienste wie IUserService
        using (var scope = context.RequestServices.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Verwenden Sie den userService innerhalb der Middleware wie benötigt
            // Beispiel: Abrufen des Benutzers basierend auf JWT-Token-Informationen (wenn vorhanden)
        }

        await _next(context);
    }
}}