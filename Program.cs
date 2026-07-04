var builder = WebApplication.CreateBuilder(args);

// ==========================
// CONTROLLERS
// ==========================
builder.Services.AddControllers();


// ==========================
// EXERCISE 3 — OPTIONS PATTERN (PaymentOptions)
// ==========================
builder.Services
    .AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

// ==========================
// PIPELINE
// ==========================
app.UseHttpsRedirection();

app.MapControllers();

app.Run(); 