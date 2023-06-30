using Application.Contracts.Constants.Rating;
using Application.Contracts.Requests.Rating;
using FluentValidation;

namespace Application.Contracts.Validations.Rating;

public class CreateRatingRequestValidator : AbstractValidator<CreateRatingRequest>
{
    public CreateRatingRequestValidator()
    {
        RuleFor(x => x.Score)
            .NotNull()
            .NotEmpty()
            .WithMessage(RatingValidationMessages.ScoreRequired)
            .ExclusiveBetween(0, 10)
            .WithMessage(RatingValidationMessages.ScoreRange);

        RuleFor(x => x.Description)
            .Length(3, 250)
            .WithMessage(RatingValidationMessages.DescriptionLength);
    }
}