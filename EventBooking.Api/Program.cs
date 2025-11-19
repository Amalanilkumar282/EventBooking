using EventBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EventBooking.Application.Mapping;
using MediatR;
using EventBooking.Application.Features.Events.Commands;
using EventBooking.Application.Interfaces;
using EventBooking.Infrastructure.Reposiories;
using FluentValidation;
using EventBooking.Application.Validators;
using EventBooking.Application.Behaviors;
using EventBooking.Api.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

// Register MediatR - handlers live in Application assembly
services.AddMediatR(typeof(CreateEventCommand).Assembly);

// Register MediatR validation behavior so validators run automatically (defense-in-depth)
services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Add OpenAPI / Swagger services
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Use global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Seed data on startup (development only)
using (var scope = app.Services.CreateScope())
{
    var servicesScope = scope.ServiceProvider;
    try
    {
        var db = servicesScope.GetRequiredService<EventBookingDbContext>();
        // apply any pending migrations (safe for development)
        db.Database.Migrate();
        // seed initial data
        DataSeeder.SeedAsync(db).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        // If seeding fails, log error to console. In production use a logger.
        Console.WriteLine($"Error seeding database: {ex}");
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

app.UseAuthorization();

app.MapControllers();

app.Run();
