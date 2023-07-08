using CachingWebAPI.Models;
using CachingWebAPI.Services;

namespace CachingWebAPI.Data;

public class DriversWithCacheProvider : IDriversProvider
{
    private readonly AppDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public DriversWithCacheProvider(AppDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<Driver>?> GetAllAsync()
    {
        var drivers = await _cacheService.GetDataAsync<IEnumerable<Driver>>("All_Drivers");
        if (drivers != null && drivers.Any())
        {
            return drivers;
        }

        drivers = _dbContext.Drivers.ToList();
        if (drivers != null && drivers.Any())
        {
            DateTimeOffset expiryTime = DateTimeOffset.Now.AddSeconds(60);
            await _cacheService.SetDataAsync("All_Drivers", drivers, expiryTime);
        }

        return drivers;
    }

    public async Task<Driver?> GetByNumberAsync(string number)
    {
        var driver = await _cacheService.GetDataAsync<Driver>($"Driver_{number}");
        if (driver != null)
        {
            return driver;
        }

        driver = _dbContext.Drivers.FirstOrDefault(d => d.Number == number);
        if (driver != null)
        {
            DateTimeOffset expiryTime = DateTimeOffset.Now.AddSeconds(30);
            await _cacheService.SetDataAsync($"Driver_{number}", driver, expiryTime);
        }

        return driver;
    }
}
