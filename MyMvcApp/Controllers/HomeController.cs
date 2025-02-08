using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;

namespace MyMvcApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TelemetryClient _telemetryClient;

    public HomeController(ILogger<HomeController> logger, TelemetryClient telemetryClient)
    {
        _logger = logger;
        _telemetryClient = telemetryClient;
    }

    public IActionResult Index()
    {
        try 
        {
            Console.WriteLine("=== Sending telemetry from Index action ===");
            
            // Базова подія
            _telemetryClient.TrackEvent("HomePageVisited");
            Console.WriteLine("Tracked event: HomePageVisited");

            // Метрика
            _telemetryClient.TrackMetric("HomePageLoadTime", 100);
            Console.WriteLine("Tracked metric: HomePageLoadTime");

            // Трасування
            _telemetryClient.TrackTrace("Test trace message", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
            Console.WriteLine("Tracked trace message");

            return View();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Index action: {ex.Message}");
            _telemetryClient.TrackException(ex);
            throw;
        }
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation("Відвідування сторінки Privacy");
        return View();
    }

    public IActionResult TestError()
    {
        try
        {
            _telemetryClient.TrackTrace("Спроба викликати тестову помилку");
            throw new Exception("Тестова помилка для Application Insights");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Сталася тестова помилка");
            _telemetryClient.TrackException(ex);
            throw;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
