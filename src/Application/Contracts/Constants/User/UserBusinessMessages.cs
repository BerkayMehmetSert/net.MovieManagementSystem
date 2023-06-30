namespace Application.Contracts.Constants.User;

public static class UserBusinessMessages
{
    public const string PasswordNotMatchWithOldPassword = "Password not match with old password.";
    public const string PasswordNotMatchWithConfirmPassword = "Password not match with confirm password.";
    public const string UserNotFoundById = "User not found by ID.";
    public const string UserNotFoundByEmail = "User not found by email.";
    public const string UserAlreadyExistByEmail = "User already exist by email.";
    public const string InvalidPassword = "Invalid password.";
}