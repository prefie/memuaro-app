using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace MemuaroApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAuthButton")]
    public ContentResult Get() // TODO: довести до ума
    {
        return base.Content(Environment.GetEnvironmentVariable("SecretHtml") ?? string.Empty, "text/html");
    }

    [HttpPost("index")]
    public string Post([FromForm] string credential) // TODO: довести до ума
    {
        var token = new JwtSecurityToken(credential);
        var name = token.Payload["name"].ToString();
        return $"Welcome, {name}";
    }
}
