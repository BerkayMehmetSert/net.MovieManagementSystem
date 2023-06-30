using Application.Contracts.Constants.Movie;
using Application.Contracts.Requests.Movie;
using FluentValidation;

namespace Application.Contracts.Validations.Movie;

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        RuleFor(x=>x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage(MovieValidationMessages.TitleRequired)
            .Length(3, 50)
            .WithMessage(MovieValidationMessages.TitleLength);
        
        RuleFor(x=>x.Plot)
            .NotNull()
            .NotEmpty()
            .WithMessage(MovieValidationMessages.PlotRequired)
            .Length(3, 500)
            .WithMessage(MovieValidationMessages.PlotLength);
        
        RuleFor(x=>x.ReleaseDate)
            .NotNull()
            .NotEmpty()
            .WithMessage(MovieValidationMessages.ReleaseDateRequired);
        
        RuleFor(x=>x.MovieLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(MovieValidationMessages.MovieLengthRequired);
    }
}