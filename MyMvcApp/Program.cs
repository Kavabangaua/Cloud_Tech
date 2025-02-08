var builder = WebApplication.CreateBuilder(args);

// Додаємо більше діагностичного логування
var connectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
Console.WriteLine("=== Application Insights Configuration ===");
Console.WriteLine($"Connection String: {connectionString}");
Console.WriteLine($"Is null or empty: {string.IsNullOrEmpty(connectionString)}");

// Додаємо явну конфігурацію
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.EnableAdaptiveSampling = false;
    options.EnableQuickPulseMetricStream = true;
    options.ConnectionString = connectionString;
    options.InstrumentationKey = "d56d3dd9-2b76-4525-bcbd-32d87ddc8abb"; // Додаємо явно
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

