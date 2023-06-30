namespace Application.Contracts.Constants.User;

public static class UserValidationMessages
{
    public const string EmailRequired = "Email is required";
    public const string EmailInvalid = "Email is invalid";
    public const string PasswordRequired = "Password is required";
    public const string FirstNameRequired = "First name is required";
    public const string FirstNameLength = "First name must be between 3 and 50 characters";
    public const string LastNameRequired = "Last name is required";
    public const string LastNameLength = "Last name must be between 3 and 50 characters";
}