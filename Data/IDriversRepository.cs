using CachingWebAPI.Models;

namespace CachingWebAPI.Data;

public interface IDriversRepository
{
    Task<Driver?> AddAsync(Driver driver);
    Task<IEnumerable<Driver>?> AddCollectionAsync(IEnumerable<Driver> drivers);
    Task<bool> RemoveAsync(int id);
}

