using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://keycloak:8080/realms/BionicPRO"; 
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = true,
            ValidIssuer = "http://keycloak:8080/realms/BionicPRO", 
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = "roles",
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProtheticUserOnly", policy =>
        policy.RequireClaim("roles", "prothetic_user"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/reports", (ClaimsPrincipal user) =>
{
    var username = user.Identity?.Name ?? "unknown_user";

    var mockReport = new
    {
        user = username,
        generatedAt = DateTime.UtcNow,
        signalStrength = new[] { 75, 80, 73 },
        actuatorEvents = new[] { "flex", "extend", "hold" }
    };

    return Results.Ok(mockReport);
}).RequireAuthorization("ProtheticUserOnly");

app.Run();