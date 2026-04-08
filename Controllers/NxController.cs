using Microsoft.AspNetCore.Mvc;
using nxtool;

[ApiController]
[Route("api/nx")]
public class NxController : ControllerBase
{
    private readonly NxManufService _service;

    public NxController(NxManufService service)
    {
        _service = service;
    }

    // Example: /api/nx/device-id?manuf=pci-id-batam/edge2-pro&index=1234
    [HttpGet("device-id")]
    public IActionResult GenerateDeviceId(string manuf, int index)
    {
        var result = _service.RunTool($"gen-device-id {manuf} {index}");
        return Ok(new { deviceId = result });
    }

    // Example: /api/nx/box-id?manuf=pci-id-batam/edge2-se&index=0
    [HttpGet("box-id")]
    public IActionResult GenerateBoxId(string manuf, int index)
    {
        var result = _service.RunTool($"gen-box-id {manuf} {index}");
        return Ok(new { boxId = result });
    }
}
