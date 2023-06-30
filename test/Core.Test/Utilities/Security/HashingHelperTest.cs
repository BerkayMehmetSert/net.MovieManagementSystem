using Core.Utilities.Security;
using Xunit;

namespace Core.Test.Utilities.Security;

public class HashingHelperTest
{
    [Fact]
    public void CreatePasswordHashCorrectPasswordShouldReturnSuccess()
    {
        const string password = "P@ssw0rd";
        HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        Assert.NotNull(passwordHash);
        Assert.NotEmpty(passwordHash);
        Assert.NotNull(passwordSalt);
        Assert.NotEmpty(passwordSalt);
    }
    
    [Fact]
    public void VerifyPasswordHashCorrectPasswordShouldReturnSuccess()
    {
        const string password = "P@ssw0rd";
        HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        var result = HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);
        Assert.True(result);
    }

    [Fact]
    public void VerifyPasswordHashInCorrectPasswordShouldReturnFalse()
    {
        const string password = "P@ssw0rd";
        const string wrongPassword = "WrongP@ssw0rd";
        HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        var result = HashingHelper.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);
        Assert.False(result);
    }
}