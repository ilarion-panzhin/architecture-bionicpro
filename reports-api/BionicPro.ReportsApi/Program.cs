using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")   
            .AllowAnyHeader()                         
            .AllowAnyMethod()                         
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://keycloak:8080/realms/reports-realm"; 
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = true,
            ValidIssuers =
            [
                "http://localhost:8080/realms/reports-realm",
                "http://keycloak:8080/realms/reports-realm"
            ],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = "realm_access.roles",
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProtheticOnly", policy =>
        policy.RequireAssertion(context =>
        {
            var rolesClaim = context.User.FindFirst("realm_access");
            if (rolesClaim == null) return false;

            var roles = System.Text.Json.JsonDocument.Parse(rolesClaim.Value)
                .RootElement
                .GetProperty("roles")
                .EnumerateArray()
                .Select(r => r.GetString());

            return roles.Contains("prothetic_user");
        }));
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); 

app.Run();