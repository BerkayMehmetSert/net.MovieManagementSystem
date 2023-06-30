namespace Application.Contracts.Constants.Rating;

public static class RatingValidationMessages
{
    public const string ScoreRequired = "Score is required";
    public const string ScoreRange = "Score must be between 0.00 and 10.00";
    public const string DescriptionLength = "Description must be between 3 and 250 characters";
}