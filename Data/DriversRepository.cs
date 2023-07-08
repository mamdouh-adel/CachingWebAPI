using CachingWebAPI.Models;

namespace CachingWebAPI.Data;

public class DriversRepository : IDriversRepository
{
    private readonly AppDbContext _dbContext;

    public DriversRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Driver?> AddAsync(Driver driver)
    {
        var driverEntity = await _dbContext.Drivers.AddAsync(driver);
        var count = await _dbContext.SaveChangesAsync();
        if (count > 0)
        {
            return driverEntity?.Entity;
        }

        return default;
    }

    public async Task<IEnumerable<Driver>?> AddCollectionAsync(IEnumerable<Driver> drivers)
    {
        await _dbContext.Drivers.AddRangeAsync(drivers);
        var count = await _dbContext.SaveChangesAsync();
        if (count > 0)
        {
            return drivers;
        }

        return default;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        Driver? driver = await _dbContext.Drivers.FindAsync(id);
        if (driver == null)
            return false;

        _dbContext.Drivers.Remove(driver);

        return true;
    }
}
