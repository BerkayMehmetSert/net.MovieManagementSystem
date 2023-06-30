using Core.Test.FakeData;
using Core.Utilities.Security;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class UserFakeData : BaseFakeData<User>
{
    public override List<User> CreateFakeData()
    {
        HashingHelper.CreatePasswordHash("1234", out var passwordHash, out var passwordSalt);
        return new List<User>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Email = "admin@system.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = "System",
                LastName = "System",
                Role = "Admin",
            },
            new()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Email = "user@system.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = "User",
                LastName = "System",
                Role = "User",
            }
        };
    }
}