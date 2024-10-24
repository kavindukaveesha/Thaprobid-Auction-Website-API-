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
using api.Service;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using api.Dto.EmailDto;
using api.Services;
using Newtonsoft.Json;
using api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ========================
// 1. Add Services to the Container
// ========================

// Add Identity services with custom configuration (UserManager and RoleManager)
// builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
// {
//     options.SignIn.RequireConfirmedAccount = false; // Set to true if you want to require confirmed accounts
//     options.SignIn.RequireConfirmedEmail = true; // This is correct for requiring email confirmation
//     options.Lockout.AllowedForNewUsers = true;
//     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(4);
//     options.Lockout.MaxFailedAccessAttempts = 5;

//     // Configure password policy
//     options.Password.RequireDigit = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequireNonAlphanumeric = true;
//     options.Password.RequiredLength = 12;
// })
//.AddEntityFrameworkStores<ApplicationDBContext>() // Ensure this matches your DbContext
//.AddDefaultTokenProviders(); // This includes the default token providers for password reset, email confirmation, etc.
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
        ValidateIssuer = true,  // Ensure token has a valid issuer
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,  // Ensure token is intended for this audience
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,  // Validate signing key
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),  // Key used to sign token
        ValidateLifetime = true  // Validate expiration
    };
});

// Add NewtonsoftJson support for handling JSON serialization issues (e.g., reference loops)
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Formatting = Formatting.Indented;
    });



// Add DbContext for EF Core with SQL Server configuration
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Swagger Authentication
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Register repositories for dependency injection (DI)
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IAuctionLotRepository, AuctionLotItemRepository>();
builder.Services.AddScoped<ItockenService, TokenService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailRepository, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IItemBidderRepository, ItemBiddedRepository>();
builder.Services.AddSingleton<MobileVerificationService>();


// Register custom global exception handlers
builder.Services.AddSingleton<GlobalExceptionHandler>();
builder.Services.AddSingleton<BadRequestExceptionHandler>();
builder.Services.AddSingleton<NotFoundExceptionHandler>();

// Enable Problem Details middleware (for standard API error responses)
builder.Services.AddProblemDetails();

// Add CORS policy (allowing all origins for now, can be customized)
// Add CORS policy (allow specific origin)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// ========================
// 2. Configure Middleware Pipeline
// ========================
var app = builder.Build();

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Enable HTTP Strict Transport Security in production
}

app.UseHttpsRedirection();  // Ensure HTTPS redirection
app.UseRouting();           // Enable routing
app.UseCors("AllowFrontend");  // Use CORS policy
app.UseAuthentication();     // Enable authentication
app.UseAuthorization();      // Enable authorization

app.MapControllers();        // Map controllers

app.Run();
