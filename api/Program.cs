using api.data;
using api.Handlers;
using api.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Interfaces;
using api.Interfaces;

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
//add identtity
// Repository
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Exception Handlers
builder.Services.AddSingleton<GlobalExceptionHandler>();
builder.Services.AddSingleton<BadRequestExceptionHandler>();
builder.Services.AddSingleton<NotFoundExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")  // React app URL
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();