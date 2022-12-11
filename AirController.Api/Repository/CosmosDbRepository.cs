using AirController.Api.Models;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using AirController.Api.Configuration;

namespace AirController.Api.Repository
{
    public class CosmosDbRepository : IRepository
    {
        private IConfiguration _config;
        private IMongoClient _mongoClient;
        private readonly IMemoryCache _cacher;

        public CosmosDbRepository(IConfiguration config, IMemoryCache cacher)
        {
            _config = config;
            _mongoClient = new MongoClient(_config.GetConnectionString("DefaultConnection"));
            _cacher = cacher;
        }

        public async Task<IEnumerable<AirPollutionAnalytic>> GetAllDataAsync()
        {
            if (_cacher.TryGetValue(GlobalConfig.AirPollutionCacheKey, out IEnumerable<AirPollutionAnalytic> cachedElems))
                return cachedElems;

            var database = _mongoClient.GetDatabase(GlobalConfig.AirPollutionDatabaseName);
            var allCursorData  = await database
                .GetCollection<AirPollutionAnalytic>(GlobalConfig.AirPollutionCollectionName)
                .FindAsync(_ => true);
            var data = await allCursorData.ToListAsync();
            var orderedData = data.OrderByDescending(x => x.Status);

            _cacher.Set(GlobalConfig.AirPollutionCacheKey, orderedData,
                new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return orderedData;
        }

        public async Task InsertAirPollutionAsync(AirPollutionAnalytic pollutionAnalytic)
        {
            (_cacher as MemoryCache).Compact(1.0);

            pollutionAnalytic.Id = Guid.NewGuid().ToString();
            pollutionAnalytic.LastModified = DateTime.Now;

            var database = _mongoClient.GetDatabase(GlobalConfig.AirPollutionDatabaseName);
            await database
                .GetCollection<AirPollutionAnalytic>(GlobalConfig.AirPollutionCollectionName)
                .InsertOneAsync(pollutionAnalytic);
        }

        public async Task DeleteForIdAsync(string guid)
        {
            (_cacher as MemoryCache).Compact(1.0);

            var database = _mongoClient.GetDatabase(GlobalConfig.AirPollutionDatabaseName);
            await database
                .GetCollection<AirPollutionAnalytic>(GlobalConfig.AirPollutionCollectionName)
                .DeleteOneAsync(x => x.Id.Equals(guid));

        }

        public async Task InsertTemperatureAsync(TemperatureAnalyticModel pollutionAnalytic)
        {
            (_cacher as MemoryCache).Compact(1.0);

            pollutionAnalytic.Id = Guid.NewGuid().ToString();

            var database = _mongoClient.GetDatabase(GlobalConfig.TemperatureDatabaseName);

            var collection = database.GetCollection<TemperatureAnalyticModel>(GlobalConfig.TemperatureCollectionName);

            await collection.DeleteManyAsync(_ => true);

            await collection.InsertOneAsync(pollutionAnalytic);
        }

        public async Task<TemperatureAnalyticModel> GetTemperatureAnalyticItemAsync()
        {
            if (_cacher.TryGetValue(GlobalConfig.TemperatureCacheKey, out TemperatureAnalyticModel cachedElems))
                return cachedElems;

            var database = _mongoClient.GetDatabase(GlobalConfig.TemperatureDatabaseName);
            var allCursorData = await database
                .GetCollection<TemperatureAnalyticModel>(GlobalConfig.TemperatureCollectionName)
                .FindAsync(_ => true);
            var data = await allCursorData.FirstAsync();

            _cacher.Set(GlobalConfig.TemperatureCacheKey, data,
                new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return data;
        }

        public SongModel? GetSong()
        {
			if (_cacher.TryGetValue(GlobalConfig.MelodyCacheKey, out SongModel songModel))
				return songModel;

            return new SongModel();
		}

		public void SetSong(SongModel song)
		{
			_cacher.Set(GlobalConfig.MelodyCacheKey, song,
			new MemoryCacheEntryOptions()
			.SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
		}
	}
}
