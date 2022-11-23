using Microsoft.Extensions.Logging;
using System;

namespace CancellationTutorial.CoreLib
{
    #region Entity

    public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }


    #endregion

    #region Repository

    public interface IRepository
    {
        Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken = default);
    }

    public class Repository : IRepository
    {
        private readonly ILogger<Repository> _logger;
        private readonly string[] summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        public Repository(ILogger<Repository> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken = default)
        {
            var list = new List<WeatherForecast>();

            try
            {
                for (int i = 1; i < 10; i++)
                {
                    #region 01
                    //if (cancellationToken.IsCancellationRequested)
                    //{
                    //    _logger.LogWarning($"Task is manual canceled.");
                    //    break;
                    //}
                    //Thread.Sleep(TimeSpan.FromSeconds(i));
                    #endregion

                    #region 02
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    #endregion

                    list.Add(new WeatherForecast(
                        DateTime.Now.AddDays(i),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]));

                    _logger.LogInformation($"Task delay {i} seconds with {list.Count} items");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Throw An Exception:{ex.Message}");
            }
            return await Task.FromResult(list);
        }
    }

    #endregion
}