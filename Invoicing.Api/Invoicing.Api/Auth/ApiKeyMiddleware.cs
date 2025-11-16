using Invoicing.Api.Repositories;

namespace Invoicing.Api.Auth
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HEADER_NAME = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthRepository authRepo)
        {
            
            if (!context.Request.Headers.TryGetValue(HEADER_NAME, out var extractedKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { error = "Missing API key." });
                return;
            }

            
            if (!Guid.TryParse(extractedKey, out Guid apiKeyGuid))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid API key format." });
                return;
            }

            
            var user = await authRepo.GetUserByApiKeyAsync(apiKeyGuid);
            if (user == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized API key." });
                return;
            }

           
            context.Items["ApiUser"] = user;

            
            await _next(context);
        }
    }
}
