using Scalar.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// SERVICES
// ==========================
builder.Services.AddControllers();

// FIX: correct OpenAPI registration
builder.Services.AddOpenApi();

// ProblemDetails (Exercise 6)
builder.Services.AddProblemDetails();

var app = builder.Build();


// ==========================
// ENVIRONMENT TOGGLE
// ==========================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseExceptionHandler();
    app.UseStatusCodePages();
}


// ==========================
// PIPELINE
// ==========================
app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/api/error", () =>
{
    throw new TmsDatabaseException(
        "Simulated database failure for ProblemDetails testing"
    );
});

app.Run();