using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nxtool.Models;
using nxtool.Services;

namespace nxtool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("add")]
        [AllowAnonymous]
        public IActionResult AddToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.PlainToken))
                return BadRequest("plainToken is required.");

            _tokenService.SaveToken(request.PlainToken);
            return Ok("Token saved successfully!");
        }

        [HttpPost("validate")]
        [AllowAnonymous]
        public IActionResult ValidateToken([FromBody] TokenRequest request)
        {
            var isValid = _tokenService.ValidateToken(request.PlainToken);
            return Ok(isValid ? "Valid token" : "Invalid token");
        }
    }
}
