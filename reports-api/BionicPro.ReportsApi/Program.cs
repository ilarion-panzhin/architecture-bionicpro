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
    options.AddPolicy("ProtheticOnly", policy =>
        policy.RequireClaim("roles", "prothetic_user"));
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); 

app.Run();