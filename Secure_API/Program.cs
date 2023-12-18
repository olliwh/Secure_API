using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Secure_API;
using Secure_API.Context;
using Secure_API.Repositories;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var optionsBuilder =
    new DbContextOptionsBuilder<UserDBContext>();
optionsBuilder.UseSqlServer(Secrets.connectionString);
UserDBContext context =
    new UserDBContext(optionsBuilder.Options);

builder.Services.AddSingleton<IUsersRepository>(
    new UsersRepository(context));
builder.Services.AddSingleton<ILoginRepository>(
    new LoginRepository(context));
builder.Services.AddSingleton<IAdminRepository>(
    new AdminRepository(context));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", 
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
});


var tokenValidationeParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = false,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ClockSkew = TimeSpan.Zero,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Secrets.SekretKey))
};
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.TokenValidationParameters = tokenValidationeParameters;

});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(1),
            }));
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

//added
app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
