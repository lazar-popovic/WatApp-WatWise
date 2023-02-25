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
}