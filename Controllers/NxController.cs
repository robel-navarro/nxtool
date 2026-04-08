using Microsoft.AspNetCore.Mvc;
using nxtool;
using nxtool.Data;
using nxtool.Models;
using nxtool.Services;

[ApiController]
[Route("api/nx")]
public class NxController : ControllerBase
{
    private readonly NxManufService _service;
    private readonly NxToolContext _context;

    public NxController(NxManufService service, NxToolContext context)
    {
        _service = service;
        _context = context;
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
    [HttpPost("add")]
    public IActionResult AddToken()
    {
        var token = new TokenRecord
        {
            HashedToken = "abc123",
            ExpiryDate = DateTime.UtcNow.AddYears(1)
        };

        _context.Tokens.Add(token);
        _context.SaveChanges();

        return Ok("Token saved successfully!");
    }

    [HttpGet("list")]
    public IActionResult GetTokens()
    {
        var tokens = _context.Tokens.ToList();
        return Ok(tokens);
    }
}
