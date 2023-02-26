using API.Data;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller;

[Controller]
[Route("[controller]")]
public class DevicesController : ControllerBase
{
    private DataContext _connection;
    
    public DevicesController(DataContext connection)
    {
        this._connection = connection;
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
        return Ok(this.getDevices());
    }
    
    [HttpPost]
    public async Task<IActionResult> postDevice([FromBody] Device device) 
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        await _connection.Devices.AddAsync( device);
        await _connection.SaveChangesAsync();
        
        return Ok(this.getDevices());
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
}