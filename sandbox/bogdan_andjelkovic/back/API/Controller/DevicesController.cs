using API.Data;
using API.Model;
using API.Services.DevicesDataGenerator.Implementation;
using API.Services.DevicesDataGenerator.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace API.Controller;

[Controller]
[Route("[controller]")]
public class DevicesController : ControllerBase
{
    private DataContext _connection;
    private IDevicesDataGenerator _devicesDataGenerator;
    
    public DevicesController(DataContext connection, IDevicesDataGenerator devicesDataGenerator)
    {
        this._connection = connection;
        _devicesDataGenerator = devicesDataGenerator;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> getDevices()
    {
        return await this._connection.Devices.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> getDevice(int id)
    {
        return await this._connection.Devices.FindAsync(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteDevice(int id)
    {
        Device device = await _connection.Devices.FindAsync(id);
        if (device == null)
        {
            return NotFound();
        }

        _connection.Devices.Remove(device);
        await _connection.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> postDevice([FromBody] Device device) 
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        await _connection.Devices.AddAsync( device);
        await _connection.SaveChangesAsync();
        
        return Ok(device);
    }
    
    [HttpPut]
    public async Task<IActionResult> updateDevice([FromBody]Device device) 
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var existingProduct = await _connection.Devices.FirstOrDefaultAsync(p => p.Id == device.Id);

        if (existingProduct == null) {
            return NotFound();
        }

        existingProduct.Name = device.Name;
        existingProduct.Price = device.Price;

        await _connection.SaveChangesAsync();

        return Ok(existingProduct);
    }
    
    [HttpGet]
    [Route("consumption")]
    public async Task<IActionResult> getDevice( DateTime startingDate, DateTime endingDate)
    {
        var result = await _devicesDataGenerator.GetUsageBetweenDates("fridge", startingDate, endingDate);
        return Ok(result);
    }

    [HttpGet]
    [Route("proba")]
    public async Task<IActionResult> proba()
    {
        BsonDateTime bsonDateTime = new BsonDateTime(DateTime.UtcNow);
        return Ok(bsonDateTime.ToUniversalTime());
    }
}