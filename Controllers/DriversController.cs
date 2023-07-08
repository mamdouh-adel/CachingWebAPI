using CachingWebAPI.Data;
using CachingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CachingWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly IDriversProvider _provider;
    private readonly IDriversRepository _repository;

    public DriversController(ILogger<DriversController> logger,
       IDriversProvider provider,
       IDriversRepository repository)
    {
        _logger = logger;
        _provider = provider;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<Driver>?> GetAll()
    {
        return await _provider.GetAllAsync();
    }

    [HttpGet]
    [Route("{number}")]
    public async Task<Driver?> GetByNumber(string number)
    {
        return await _provider.GetByNumberAsync(number);
    }

    [HttpPost]
    public async Task<Driver?> PostDriver([FromBody] Driver driver)
    {
        return await _repository.AddAsync(driver);
    }

    [HttpPost]
    [Route("collection")]
    public async Task<IEnumerable<Driver>?> PostDrivers([FromBody] IEnumerable<Driver> drivers)
    {
        return await _repository.AddCollectionAsync(drivers);
    }

    [HttpDelete]
    public async Task<bool> RemoveDriver(int id)
    {
        return await _repository.RemoveAsync(id);
    }
}
