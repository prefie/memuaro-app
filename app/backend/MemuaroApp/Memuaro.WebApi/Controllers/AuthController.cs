using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Models;
using Memuaro.Persistance.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace Memuaro.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("login")]
    public async Task<User> Login([FromQuery] string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            user = new User(Guid.NewGuid(), email, new HashSet<Role>{Role.User});
            await _userRepository.CreateAsync(user);
        }
        
        return user;
    }
}