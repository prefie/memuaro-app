using Rememory.Auth;
using Rememory.Persistance.Repositories.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rememory.Persistance.Models;
using Rememory.Persistance.Repositories.UserSettingsRepository;
using Rememory.WebApi.Dtos.User;
using Rememory.WebApi.Exceptions;

namespace Rememory.WebApi.Controllers;

[Authorize]
public class UsersController : BaseController
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSettingsRepository _userSettingsRepository;

    public UsersController(
        AuthProvider authProvider,
        IUserRepository userRepository,
        IUserSettingsRepository userSettingsRepository) : base(authProvider)
    {
        _userRepository = userRepository;
        _userSettingsRepository = userSettingsRepository;
    }

    [HttpGet]
    [Route("current")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var accessToken = _authProvider.ParseAuthHeader(HttpContext.Request.Headers.Authorization);
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

    [HttpGet]
    [Route("settings/address")]
    public async Task<ActionResult<AddressSettings>> GetAddressSettings([FromQuery] Guid userId)
    {
        CheckAccessForUser(userId);
        var addressSettings = await _userSettingsRepository.GetAddressSettings(userId);
        return Ok(addressSettings);
    }

    [HttpPost]
    [Route("settings/address")]
    public async Task<IActionResult> SetAddressSettings([FromQuery] Guid userId,
        [FromBody] AddressSettings addressSettings)
    {
        CheckAccessForUser(userId);
        await _userSettingsRepository.SetAddressSettings(userId, addressSettings);
        return Ok();
    }

    private void CheckAccessForUser(Guid userId)
    {
        var accessToken = _authProvider.ParseAuthHeader(HttpContext.Request.Headers.Authorization);
        var userCred = _authProvider.GetCurrentUserCredential(accessToken);
        if (userCred.Id != userId) throw new ForbiddenException();
    }
}