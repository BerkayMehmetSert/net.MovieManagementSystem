using Application.Contracts.Constants.Genre;
using Application.Contracts.Requests.Genre;
using FluentValidation;

namespace Application.Contracts.Validations.Genre;

public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
{
    public CreateGenreRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(GenreValidationMessages.NameRequired)
            .Length(3, 50)
            .WithMessage(GenreValidationMessages.NameLength);
        
        RuleFor(x=>x.Description)
            .Length(3,250)
            .WithMessage(GenreValidationMessages.DescriptionLength);
    }
}