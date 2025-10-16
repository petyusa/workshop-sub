using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data;
using Workshop.Api.Extensions;
using Workshop.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<WorkshopDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy for Angular client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Migrate database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WorkshopDbContext>();
    context.Database.Migrate();
}

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sonrisa Booking API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAngularClient");
app.UseHttpsRedirection();

// Add authentication middleware (after CORS, before endpoints)
app.UseMiddleware<AuthenticationMiddleware>();

// Health check endpoint (no auth required)
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithName("HealthCheck")
   .WithOpenApi();

// Test endpoint to verify authentication
app.MapGet("/api/auth/me", (HttpContext context) =>
{
    var user = context.GetCurrentUser();
    return Results.Ok(new 
    { 
        username = user?.Username, 
        role = user?.Role,
        isAdmin = context.IsAdmin(),
        isEmployee = context.IsEmployee()
    });
})
.WithName("GetCurrentUser")
.WithOpenApi()
.WithDescription("Returns the current authenticated user information");

// Map location endpoints
app.MapLocationEndpoints();

// Map reservable object endpoints
app.MapReservableObjectEndpoints();

// Map reservation endpoints
app.MapReservationEndpoints();

app.Run();
