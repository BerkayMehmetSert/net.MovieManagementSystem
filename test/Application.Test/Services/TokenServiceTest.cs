using Application.Services;
using Core.Utilities.Security;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class TokenServiceTest
{
    private readonly Mock<IOptionsMonitor<JwtOptions>> _jwtOptionsMock = new();
    private readonly TokenService _tokenService;
    
    public TokenServiceTest()
    {
        _jwtOptionsMock.Setup(x => x.CurrentValue).Returns(new JwtOptions
        {
            Secret = "2A49DF37289D10E75308E22DD7C9C9B17826858F5DE3AF741A00B4B47C4C2353",
            Issuer = "issuer",
            Audience = "audience",
            AccessTokenExpiration = 60
        });
        _tokenService = new TokenService(_jwtOptionsMock.Object);
    }

    [Fact]
    public void CreateTokenShouldReturnTokenResponse()
    {
        var user = new User
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            FirstName = "John",
            LastName = "Doe",
            Email = "user@system.com",
            Role = "User",
        };
        var result = _tokenService.CreateToken(user);
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.Equal(user.Role, result.Role);
    }
}