﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Apis.Auth;
using Memuaro.Auth.Dtos;
using Memuaro.Auth.Exceptions;
using Memuaro.Auth.Settings;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Models;
using Memuaro.Persistance.Repositories.UserRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Memuaro.Auth;

public class AuthProvider
{
    private readonly GoogleSettings _googleSettings;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;

    public AuthProvider(
        IOptions<GoogleSettings> googleSettings,
        IOptions<JwtSettings> jwtSettings,
        IUserRepository userRepository)
    {
        _googleSettings = googleSettings.Value;
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }
    
    public async Task<GoogleJsonWebSignature.Payload?> ValidateAsync(string idToken)
    {
        try
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] {_googleSettings.ClientId}
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, validationSettings);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public UserCredentials GetCurrentUserCredential(string? accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadJwtToken(accessToken);
        var emailClaim = securityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
        var idClaim = securityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        var roles = securityToken.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(role =>
        {
            if (Enum.TryParse(typeof(Role), role.Value, out var parsedRole) && parsedRole != null)
            {
                return (Role) parsedRole;
            }

            throw new Exception();
        });
        
        if (emailClaim == null || idClaim == null) throw new UnauthorizedException();

        return new UserCredentials {Email = emailClaim.Value, Id = Guid.Parse(idClaim.Value), Roles = roles.ToHashSet()};
    }

    public async Task<UserCredentials> CheckTokens(TokensDto tokens)
    {
        var userCred = GetCurrentUserCredential(tokens.AccessToken);

        var user = await _userRepository.GetByEmailAsync(userCred.Email);
        if (user == null)
        {
            throw new UnauthorizedException();
        }

        if (user.RefreshToken != tokens.RefreshToken)
        {
            await ClearRefreshToken(user);
            throw new UnauthorizedException();
        }

        return userCred;
    }

    public TokensDto GenerateTokens(UserCredentials userCredentials)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (_jwtSettings.SecurityKey == null)
        {
            throw new Exception($"Bad server settings");
        }
        
        
        var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
        var claims = GenerateClaims(userCredentials);
        var jwtSecurityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)

        );
        var accessToken = tokenHandler.WriteToken(jwtSecurityToken);
        var refreshToken = GenerateRefreshToken();
        var tokens = new TokensDto {AccessToken = accessToken, RefreshToken = refreshToken};

        return tokens;
    }

    private IEnumerable<Claim> GenerateClaims(UserCredentials userCredentials)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, userCredentials.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Email, userCredentials.Email ?? throw new InvalidOperationException()));
        claims.AddRange(userCredentials.Roles?.Select(role => new Claim(ClaimTypes.Role, role.ToString())) ?? throw new InvalidOperationException());

        return claims;
    }

    public async Task SaveTokens(UserCredentials userCredentials, TokensDto tokens)
    {
        var user = await _userRepository.GetByEmailAsync(userCredentials.Email);
        if (user == null)
        {
            throw new UnauthorizedException();
        }
        
        user.RefreshToken = tokens.RefreshToken;
        await _userRepository.UpdateAsync(user.Id, user);
    }

    private async Task ClearRefreshToken(User user)
    {
        user.RefreshToken = null;
        await _userRepository.UpdateAsync(user.Id, user);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}