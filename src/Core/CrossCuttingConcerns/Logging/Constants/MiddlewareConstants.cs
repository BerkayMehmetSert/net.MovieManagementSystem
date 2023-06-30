namespace Core.CrossCuttingConcerns.Logging.Constants;

public static class MiddlewareConstants
{
    public static readonly List<string> SensitiveFields = new()
    {
        "password",
        "newPassword",
        "confirmPassword",
        "oldPassword",
        "creditCardCvv2",
        "creditCardExpireDate",
        "creditCardName",
        "creditCardNumber",
        "ssn",
        "accessToken",
        "Authorization",
    };
}