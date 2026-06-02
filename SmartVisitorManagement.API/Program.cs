using SmartVisitorManagement.API.Extensions;
using SmartVisitorManagement.API.Middleware;
using SmartVisitorManagement.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// ── Services Registration ────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddSwagger();

var app = builder.Build();

// ── Middleware Pipeline (ORDER MATTERS) ──────────────────────────────────────
app.UseMiddleware<GlobalExceptionMiddleware>();  // 1. Catch all unhandled exceptions
app.UseMiddleware<ApiKeyMiddleware>();           // 2. Validate API key
app.UseMiddleware<RequestLoggingMiddleware>();   // 3. Log authenticated requests

// ── Swagger ──────────────────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Visitor Management API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.MapControllers();

// ── Seed Database ────────────────────────────────────────────────────────────
await DataSeeder.SeedAsync(app.Services);

app.Run();