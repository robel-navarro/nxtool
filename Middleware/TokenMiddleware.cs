using Microsoft.AspNetCore.Authorization;
using nxtool.Services;

namespace nxtool.Middleware
{
    ////ENFORCE AUTHORIZATION VIA TOKEN
    //public class TokenMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly string _secretToken;

    //    public TokenMiddleware(RequestDelegate next, IConfiguration config)
    //    {
    //        _next = next;
    //        _secretToken = config["SecretKey"] ?? "ADMIN"; // from environment
    //    }

    //    public async Task InvokeAsync(HttpContext context)
    //    {
    //        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

    //        if (authHeader == null || !authHeader.StartsWith("Bearer "))
    //        {
    //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //            await context.Response.WriteAsync("Missing or invalid Authorization header.");
    //            return;
    //        }

    //        var token = authHeader.Substring("Bearer ".Length).Trim();

    //        // Validate against DB or hash
    //        using (var scope = context.RequestServices.CreateScope())
    //        {
    //            var tokenService = scope.ServiceProvider.GetRequiredService<TokenService>();
    //            if (!tokenService.ValidateToken(token))
    //            {
    //                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //                await context.Response.WriteAsync("Invalid token.");
    //                return;
    //            }
    //        }

    //        await _next(context);
    //    }
    //}

    // ENFORCE AUTHORIZATION VIA TOKEN BUT ALLOW ANONYMOUS ENDPOINTS TO BYPASS
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TokenService tokenService)
        {
            // Only protect /api/nx/* routes
            if (context.Request.Path.StartsWithSegments("/api/nx"))
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || !authHeader.StartsWith("Bearer "))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Missing or invalid Authorization header.");
                    return;
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Validate against DB
                if (!tokenService.ValidateToken(token))
                {

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid or expired token.");
                    return;
                }
            }

            await _next(context);
        }
    }



}
