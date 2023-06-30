namespace Application.Contracts.Constants.Award;

public static class AwardValidationMessages
{
    public const string NameRequired = "Name is required";
    public const string NameLength = "Name must be between 3 and 50 characters";
    public const string DescriptionLength = "Description must be between 3 and 250 characters";
    public const string DateRequired = "Date is required";
}