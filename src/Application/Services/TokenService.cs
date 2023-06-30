using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using Core.Utilities.Date;
using Core.Utilities.Security;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    public TokenService(IOptionsMonitor<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.CurrentValue;
    }

    public TokenResponse CreateToken(User user)
    {
        var token = GenerateJwtToken(user, DateHelper.GetCurrentDate());

        return new TokenResponse
        {
            AccessToken = token,
            ExpireTime = DateHelper.GetCurrentDate().AddMinutes(_jwtOptions.AccessTokenExpiration),
            Role = user.Role
        };
    }

    private string GenerateJwtToken(User user, DateTime dateTime)
    {
        var claims = GetClaim(user);

        var secret = SecurityKeyHelper.CreateSecurityKey(_jwtOptions.Secret);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(secret);

        var jwtToken = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: dateTime.AddMinutes(_jwtOptions.AccessTokenExpiration),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    private static IEnumerable<Claim> GetClaim(User user)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, $"{user.FirstName}"),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("FullName", $"{user.FirstName} {user.LastName}"),
            new Claim("Role", user.Role),
            new Claim("Email", user.Email),
            new Claim("UserId", user.Id.ToString())
        };
    }
}