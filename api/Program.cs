
using api.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using api.Interfaces;
using api.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using api.Dto.Auth;
using Interfaces;
using api.repository;
using api.data;
using api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add JwtConfig to the DI container
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// Identity services configuration
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
    .AddEntityFrameworkStores<ApplicationDBContext>();


// JWT services configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // For development - set to true in production and configure
        ValidateAudience = false, // For development - set to true in production and configure
        RequireExpirationTime = true,
        ValidateLifetime = true
    };
});


// Add NewtonsoftJson for connections
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Add Repositories (Make sure these are correctly registered)
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IAuctionLotRepository, AuctionLotRepository>();

// Exception Handlers
builder.Services.AddSingleton<GlobalExceptionHandler>();
builder.Services.AddSingleton<BadRequestExceptionHandler>();
builder.Services.AddSingleton<NotFoundExceptionHandler>();
builder.Services.AddProblemDetails();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Global exception handler
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var globalExceptionHandler = context.RequestServices.GetService<GlobalExceptionHandler>();
        var badRequestExceptionHandler = context.RequestServices.GetService<BadRequestExceptionHandler>();
        var notFoundExceptionHandler = context.RequestServices.GetService<NotFoundExceptionHandler>();

        if (notFoundExceptionHandler != null && await notFoundExceptionHandler.TryHandleAsync(context, exception, CancellationToken.None))
            return;
        if (badRequestExceptionHandler != null && await badRequestExceptionHandler.TryHandleAsync(context, exception, CancellationToken.None))
            return;
        if (globalExceptionHandler != null && await globalExceptionHandler.TryHandleAsync(context, exception, CancellationToken.None))
            return;
    });
});

// Use CORS policy
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();