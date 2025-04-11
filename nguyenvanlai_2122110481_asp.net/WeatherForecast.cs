namespace nguyenvanlai_2122110481_asp.net
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        // Dùng phép toán chuẩn để chuyển đổi từ Celsius sang Fahrenheit
        public int TemperatureF => 32 + (int)(TemperatureC * 9.0 / 5.0);

        public string? Summary { get; set; }
    }
}
