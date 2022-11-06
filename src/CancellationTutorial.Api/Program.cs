#region Program

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IRepository, Repository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async ([FromServices]IRepository repository) =>
{
   var forecast = await repository.GetAsync();
    return Results.Ok(new
    {
        count = forecast.Count(),
        forecast,
    });
})
.WithName("GetWeatherForecast");

app.MapGet("/weatherforecastwithCancel", async (CancellationToken cancellationToken, [FromServices] IRepository repository) =>
{
    var forecast = await repository.GetAsync(cancellationToken);
    return Results.Ok(new
    {
        count = forecast.Count(),
        forecast,
    });
})
.WithName("weatherforecastwithCancel");

app.Run();

#endregion


#region Entity

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


#endregion


#region Repository

internal interface IRepository
{
    Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken = default);
}

internal class Repository : IRepository
{
    private readonly ILogger<Repository> _logger;
    private readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Repository(ILogger<Repository> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var firstResult = await Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                return GetWeatherForecasts(5);
            }, cancellationToken);
            _logger.LogInformation("First result returned from database.");

            var secondResult = await Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(7), cancellationToken);
                return GetWeatherForecasts(7);
            }, cancellationToken);
            _logger.LogInformation("Second result returned from database.");
            return firstResult.Concat(secondResult);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Operation was cancelled.");
            return Array.Empty<WeatherForecast>();
        }
    }

    private IReadOnlyList<WeatherForecast> GetWeatherForecasts(int count)
    {
        var forecast = Enumerable.Range(1, count).Select(index =>
            new WeatherForecast
            (
                DateTime.Now.AddDays(index),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    }
}

#endregion