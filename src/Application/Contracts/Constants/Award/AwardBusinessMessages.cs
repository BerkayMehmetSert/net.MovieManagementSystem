namespace Application.Contracts.Constants.Award;

public static class AwardBusinessMessages
{
    public const string AwardAlreadyExistsByName = "Award already exists by name.";
    public const string AwardNotFoundById = "Award not found by id.";
    public const string AwardHasActors = "Award cannot be deleted because of actor awards.";
    public const string AwardNotFoundByName = "Award not found by name.";
}