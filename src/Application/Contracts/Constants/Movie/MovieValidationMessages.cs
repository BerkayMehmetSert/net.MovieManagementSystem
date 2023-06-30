namespace Application.Contracts.Constants.Movie;

public static class MovieValidationMessages
{
    public const string TitleRequired = "Title is required";
    public const string TitleLength = "Title must be between 3 and 50 characters";
    public const string PlotRequired = "Plot is required";
    public const string PlotLength = "Plot must be between 3 and 500 characters";
    public const string ReleaseDateRequired = "Release date is required";
    public const string MovieLengthRequired = "Movie length is required";
}