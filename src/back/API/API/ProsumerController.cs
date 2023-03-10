using API.BL.Interfaces;
using API.Models.Dot;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[ApiController]
[Route("api/prosumer")]
public class ProsumerController : ControllerBase
{
    private readonly IProsumerBL _prosumerBl;

    public ProsumerController( IProsumerBL prosumerBl)
    {
        _prosumerBl = prosumerBl;
    }

    [HttpPost("register")]
    public IActionResult RegisterProsumer(UserRegisterDot request)
    {
        var response = _prosumerBl.RegisterProsumer(request);

        if (response.Success)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }
}