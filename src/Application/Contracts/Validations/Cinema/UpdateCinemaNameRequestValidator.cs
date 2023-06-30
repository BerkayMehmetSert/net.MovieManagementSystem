using Application.Contracts.Constants.Cinema;
using Application.Contracts.Requests.Cinema;
using FluentValidation;

namespace Application.Contracts.Validations.Cinema;

public class UpdateCinemaNameRequestValidator : AbstractValidator<UpdateCinemaNameRequest>
{
    public UpdateCinemaNameRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(CinemaValidationMessages.NameRequired)
            .Length(3, 50)
            .WithMessage(CinemaValidationMessages.NameLength);
    }
}