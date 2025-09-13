using Microsoft.AspNetCore.Mvc;

namespace Azorian.Controllers;

/// <summary>
/// Provides HTTP endpoints for retrieving weather forecast data.
/// </summary>
/// <remarks>
/// Namespace: <c>Azorian.Controllers</c><br/>
/// Inheritance: <see cref="ControllerBase"/><br/>
/// Attributes: <c>ApiController</c> (marks this class as an API controller),
/// <c>Route("[controller]")</c> (maps route prefix based on controller name).<br/>
/// Routes for this controller:
/// <code>GET http://localhost:5197/WeatherForecast</code>
/// </remarks>
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    /// <summary>
    /// Static set of possible forecast summary strings.
    /// </summary>
    /// <value>
    /// Possible values include:
    /// <list type="bullet">
    ///   <item>Freezing</item>
    ///   <item>Bracing</item>
    ///   <item>Chilly</item>
    ///   <item>Cool</item>
    ///   <item>Mild</item>
    ///   <item>Warm</item>
    ///   <item>Balmy</item>
    ///   <item>Hot</item>
    ///   <item>Sweltering</item>
    ///   <item>Scorching</item>
    /// </list>
    /// </value>
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    /// <summary>
    /// Logger used for diagnostic output.
    /// </summary>
    private readonly ILogger<WeatherForecastController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
    /// </summary>
    /// <param name="logger">Logger instance used to record diagnostic information.</param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests to <c>/WeatherForecast</c>.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="WeatherForecast"/> objects for the next five days.
    /// </returns>
    /// <remarks>
    /// Behavior:
    /// <list type="bullet">
    ///   <item>Generates a forecast for the next 5 days.</item>
    ///   <item>Each forecast contains:
    ///     <list type="bullet">
    ///       <item><c>Date</c> – a <see cref="DateOnly"/> for the forecasted day.</item>
    ///       <item><c>TemperatureC</c> – randomized Celsius temperature between -20 and 55.</item>
    ///       <item><c>Summary</c> – a random descriptive summary from <see cref="Summaries"/>.</item>
    ///     </list>
    ///   </item>
    /// </list>
    ///
    /// Example request:
    /// <code>GET http://localhost:5197/WeatherForecast</code>
    ///
    /// Example response:
    /// <code language="json">
    /// [
    ///   { "date": "2025-09-14", "temperatureC": 25, "summary": "Warm" },
    ///   { "date": "2025-09-15", "temperatureC": 17, "summary": "Mild" }
    /// ]
    /// </code>
    /// </remarks>
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
