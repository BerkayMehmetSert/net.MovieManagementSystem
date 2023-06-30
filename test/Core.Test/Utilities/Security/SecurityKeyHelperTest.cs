using Core.Utilities.Security;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Core.Test.Utilities.Security;

public class SecurityKeyHelperTest
{
    [Fact]
    public void CreateSecurityKeyShouldReturnSuccess()
    {
        const string securityKey = "securityKey";
        var result = SecurityKeyHelper.CreateSecurityKey(securityKey);
        Assert.NotNull(result);
        Assert.IsType<SymmetricSecurityKey>(result);
    }
}