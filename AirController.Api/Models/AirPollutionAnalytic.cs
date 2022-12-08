namespace AirController.Api.Models
{
    public class AirPollutionAnalytic
    {
        public string? Id { get; set; }
        public DateTime LastModified { get; set; }
        public string? Location { get; set; }
        public int Status { get; set; }
    }
}
