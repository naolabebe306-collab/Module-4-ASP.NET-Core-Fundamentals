using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// SERVICES
// ==========================
builder.Services.AddControllers();

// THIS enables proper RFC 9457 ProblemDetails mapping
builder.Services.AddProblemDetails();

var app = builder.Build();


// ==========================
// EXCEPTION HANDLING (IMPORTANT FIX)
// ==========================
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var problem = new ProblemDetails
        {
            Title = "A server error occurred",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception?.Message,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1"
        };

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(problem);
    });
});


// ==========================
// OPTIONAL (safe)
// ==========================
app.UseStatusCodePages();

app.UseHttpsRedirection();


// ==========================
// ROUTES
// ==========================
app.MapControllers();

app.MapGet("/api/error", () =>
{
    throw new TmsDatabaseException(
        "Simulated database failure for ProblemDetails testing"
    );
});

app.Run();