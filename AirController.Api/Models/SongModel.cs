namespace AirController.Api.Models
{
    public class SongModel
    {
        public string Name { get; set; } = String.Empty;
        public List<int> Notes { get; set; } = new();
    }
}
