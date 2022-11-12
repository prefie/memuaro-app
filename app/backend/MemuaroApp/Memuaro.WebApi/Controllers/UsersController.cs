using Memuaro.Auth;
using Memuaro.Persistance.Repositories.UserRepository;
using Memuaro.WebApi.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Memuaro.WebApi.Controllers;

[Authorize]
public class UsersController : BaseController
{
    private readonly IUserRepository _userRepository;

    public UsersController(
        AuthProvider authProvider,
        IUserRepository userRepository) : base(authProvider) 
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("current")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var accessToken = HttpContext.Request.Headers.Authorization.ToArray()[0].Split()[1];
        var userCred = _authProvider.GetCurrentUserCredential(accessToken);
        var user = await _userRepository.GetByEmailAsync(userCred.Email);

        return Ok(new UserDto
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            PhotoUrl = user.PhotoUrl,
            Roles = user.Roles?.Select(role => role.ToString()),
        });
    }
}