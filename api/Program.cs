using api.Handlers;
using api.Interfaces;
using api.Dto.Auth;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Interfaces;
using api.repository;
using api.data;

var builder = WebApplication.CreateBuilder(args);

// ========================
// 1. Add Services to the Container
// ========================

// Add Identity services with custom configuration (UserManager and RoleManager)
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Configure password policy
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<ApplicationDBContext>() // Use EF Core for Identity
.AddDefaultTokenProviders(); // Token provider for password reset, etc.

// Add Authentication (JWT Bearer)
builder.Services.AddAuthentication(options =>
{
    // Configure JWT as the default scheme
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // Configure JWT validation parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Ensure token has a valid issuer
        ValidIssuer = builder.Configuration["JWT:Issuer"],

        ValidateAudience = true, // Ensure token is intended for this audience
        ValidAudience = builder.Configuration["JWT:Audience"],

        ValidateIssuerSigningKey = true, // Validate signing key
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])), // Key used to sign token

        ValidateLifetime = true // Validate expiration
    };
});

// Add NewtonsoftJson support for handling JSON serialization issues (e.g., reference loops)
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Add DbContext for EF Core with SQL Server configuration
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Swagger for API documentation in development mode
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories for dependency injection (DI)
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IAuctionLotRepository, AuctionLotRepository>();

// Register custom global exception handlers
builder.Services.AddSingleton<GlobalExceptionHandler>();
builder.Services.AddSingleton<BadRequestExceptionHandler>();
builder.Services.AddSingleton<NotFoundExceptionHandler>();

// Enable Problem Details middleware (for standard API error responses)
builder.Services.AddProblemDetails();

// Add CORS policy to allow connections from frontend (React in this case)
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

// ========================
// 2. Configure the HTTP Request Pipeline
// ========================

if (app.Environment.IsDevelopment())
{
    // Enable Swagger in development for API exploration
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handling for catching all unhandled exceptions
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        // Extract the exception details
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Resolve the handlers from DI
        var globalExceptionHandler = context.RequestServices.GetService<GlobalExceptionHandler>();
        var badRequestExceptionHandler = context.RequestServices.GetService<BadRequestExceptionHandler>();
        var notFoundExceptionHandler = context.RequestServices.GetService<NotFoundExceptionHandler>();

        // Use specific exception handlers in order of priority
        if (notFoundExceptionHandler != null && await notFoundExceptionHandler.TryHandleAsync(context, exception, CancellationToken.None))
            return;

        if (badRequestExceptionHandler != null && await badRequestExceptionHandler.TryHandleAsync(context, exception, CancellationToken.None))
            return;

        if (globalExceptionHandler != null && await globalExceptionHandler.TryHandleAsync(context, exception, CancellationToken.None))
            return;
    });
});

// Enable CORS for requests from the React frontend
app.UseCors("AllowReactApp");

// Use HTTPS redirection for all requests
app.UseHttpsRedirection();

// Use Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();
