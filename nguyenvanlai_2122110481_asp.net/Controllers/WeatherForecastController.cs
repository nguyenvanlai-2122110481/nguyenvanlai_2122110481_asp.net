using Microsoft.AspNetCore.Mvc;

namespace nguyenvanlai_2122110481_asp.net.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static List<WeatherForecast> _forecasts = new List<WeatherForecast>();
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // L?y danh sách d? báo th?i ti?t
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return _forecasts.Any()
                ? _forecasts
                : Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        // T?o m?i d? báo th?i ti?t
        [HttpPost]
        public IActionResult Create([FromBody] WeatherForecast forecast)
        {
            if (forecast == null)
                return BadRequest("D? báo không h?p l?.");

            forecast.Date = DateTime.Now.AddDays(_forecasts.Count + 1);
            _forecasts.Add(forecast);
            return Ok(forecast);
        }

        // C?p nh?t d? báo th?i ti?t
        [HttpPut("{index}")]
        public IActionResult Update(int index, [FromBody] WeatherForecast updatedForecast)
        {
            if (updatedForecast == null)
                return BadRequest("D? báo không h?p l?.");

            if (index < 0 || index >= _forecasts.Count)
                return NotFound("Không tìm th?y m?c c?n s?a.");

            _forecasts[index].TemperatureC = updatedForecast.TemperatureC;
            _forecasts[index].Summary = updatedForecast.Summary;
            return Ok(_forecasts[index]);
        }

        // Xóa d? báo th?i ti?t
        [HttpDelete("{index}")]
        public IActionResult Delete(int index)
        {
            if (index < 0 || index >= _forecasts.Count)
                return NotFound("Không tìm th?y m?c c?n xóa.");

            var removed = _forecasts[index];
            _forecasts.RemoveAt(index);
            return Ok(removed);
        }
    }
}
