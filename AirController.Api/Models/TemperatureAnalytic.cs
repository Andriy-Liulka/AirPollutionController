namespace AirController.Api.Models
{
    public class TemperatureAnalyticModel
    {
        public string? Id { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
        public float Altitude { get; set; }
    }
}
