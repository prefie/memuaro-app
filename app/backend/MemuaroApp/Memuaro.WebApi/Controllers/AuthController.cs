using Memuaro.Auth;
using Memuaro.Auth.Dtos;
using Memuaro.Auth.Exceptions;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Models;
using Memuaro.Persistance.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Memuaro.WebApi.Controllers;

public class AuthController : BaseController
{
    private readonly IUserRepository _userRepository;

    public AuthController(
        IUserRepository userRepository,
        AuthProvider authProvider) : base(authProvider)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromQuery] string idToken)
    {
        var payload = await _authProvider.ValidateAsync(idToken);
        if (payload == null || payload.Email.IsNullOrEmpty())
        {
            throw new UnauthorizedException("Bad payload");
        }

        var email = payload.Email;

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            user = new User(Guid.NewGuid(), email, payload.Name, payload.Picture, new HashSet<Role> {Role.User});
            await _userRepository.CreateAsync(user);
        }

        var userCred = new UserCredentials {Id = user.Id, Email = user.Email, Roles = user.Roles};
        var tokens = _authProvider.GenerateTokens(userCred);
        await _authProvider.SaveTokens(userCred, tokens);

        return Ok(tokens);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<TokenResponse>> RefreshTokens([FromBody] TokenResponse tokens)
    {
        var userCred = await _authProvider.CheckTokens(tokens);
        var newTokens = _authProvider.GenerateTokens(userCred);
        await _authProvider.SaveTokens(userCred, newTokens);

        return Ok(newTokens);
    }
}