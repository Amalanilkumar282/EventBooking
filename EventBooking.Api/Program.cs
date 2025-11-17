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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register validators explicitly
builder.Services.AddScoped<IValidator<CreateEventCommand>, CreateEventCommandValidator>();
builder.Services.AddScoped<IValidator<EventBooking.Application.Features.Bookings.Commands.CreateBookingCommand>, CreateBookingCommandValidator>();

// Register AutoMapper from Application assembly
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Register DbContext
builder.Services.AddDbContext<EventBookingDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("EventBooking.Infrastructure")));

// Register repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();

// Register MediatR - handlers live in Application assembly
builder.Services.AddMediatR(typeof(CreateEventCommand).Assembly);

// Add OpenAPI / Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed data on startup (development only)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<EventBookingDbContext>();
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
