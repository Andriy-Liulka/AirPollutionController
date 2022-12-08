using AirController.Api.Models;

namespace AirController.Api.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<AirPollutionAnalytic>> GetAllDataAsync();
        Task InsertAirPollutionAsync(AirPollutionAnalytic pollutionAnalytic);
        Task DeleteForIdAsync(string guid);
        Task InsertTemperatureAsync(TemperatureAnalyticModel pollutionAnalytic);
        Task<TemperatureAnalyticModel> GetTemperatureAnalyticItemAsync();
    }
}
