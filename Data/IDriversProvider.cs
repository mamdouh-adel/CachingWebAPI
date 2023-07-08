using CachingWebAPI.Models;

namespace CachingWebAPI.Data;

public interface IDriversProvider
{
    Task<Driver?> GetByNumberAsync(string number);
    Task<IEnumerable<Driver>?> GetAllAsync();
}
