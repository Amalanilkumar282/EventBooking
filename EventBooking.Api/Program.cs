using EventBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EventBooking.Application.Mapping;
using MediatR;
using EventBooking.Application.Features.Events.Commands;
using EventBooking.Application.Interfaces;
using EventBooking.Infrastructure.Reposiories;
using EventBooking.Infrastructure.Services;
using FluentValidation;
using EventBooking.Application.Validators;
using EventBooking.Application.Behaviors;
using EventBooking.Api.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

// Enable MVC and FluentValidation automatic model validation
services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Type = "https://example.com/validation-error",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest
            };

            return new BadRequestObjectResult(problemDetails);
        };
    })
    .AddFluentValidation();
// Register FluentValidation validators from the Application assembly
services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();

// Register AutoMapper from Application assembly
services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Register DbContext
services.AddDbContext<EventBookingDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("EventBooking.Infrastructure")));

// Register repositories
services.AddScoped<IEventRepository, EventRepository>();
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
services.AddScoped<IBookingRepository, BookingRepository>();

// Register authentication services
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<ITokenService, TokenService>();

// Register MediatR - handlers live in Application assembly
services.AddMediatR(typeof(CreateEventCommand).Assembly);

// Register MediatR validation behavior so validators run automatically (defense-in-depth)
services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

services.AddAuthorization();

// Add OpenAPI / Swagger services with JWT support
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "EventBooking API", 
        Version = "v1",
        Description = "Event Booking API with JWT Authentication"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: 'Bearer eyJhbGci...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Use global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Seed data on startup (development only)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var servicesScope = scope.ServiceProvider;
        try
        {
            var db = servicesScope.GetRequiredService<EventBookingDbContext>();
            
            // Apply any pending migrations (safe for development)
            await db.Database.MigrateAsync();
            
            // Seed initial data (only if database is empty)
            await DataSeeder.SeedAsync(db);
            
            Console.WriteLine("Database migration and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            // If seeding fails, log error to console. In production use a logger.
            var logger = servicesScope.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            Console.WriteLine($"Error seeding database: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swashbuckle middleware
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventBooking API V1");
        c.RoutePrefix = "openapi/ui"; // serve UI at /openapi/ui
    });
}

// keep MapOpenApi if still desired
app.MapOpenApi();

app.UseHttpsRedirection();

// IMPORTANT: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
