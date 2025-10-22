using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SyntaxCore.Infrastructure.DbContext;
using SyntaxCore.Infrastructure.Middlewares;
using SyntaxCore.Infrastructure.ServiceCollection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var license = Environment.GetEnvironmentVariable("MEDIATR_LICENSE");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDependencyService(license);
builder.Services.AddInfrastructureServices();
builder.Services.AddRepositoriesServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false; // Set to true in production
    opt.SaveToken = true; // Save the token in the AuthenticationProperties after a successful authorization
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:AccessTokenKey")!)),
        ValidateIssuerSigningKey = true,
        ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
    };
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddMvc();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await DatabaseChecker.CheckDatabaseConnection(dbContext, logger);
}

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
