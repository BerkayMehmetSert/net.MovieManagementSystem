using Application.Contracts.Constants.Cinema;
using Application.Contracts.Requests.Cinema;
using FluentValidation;

namespace Application.Contracts.Validations.Cinema;

public class CreateCinemaRequestValidator : AbstractValidator<CreateCinemaRequest>
{
    public CreateCinemaRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(CinemaValidationMessages.NameRequired)
            .Length(3, 50)
            .WithMessage(CinemaValidationMessages.NameLength);
        
        RuleFor(x=>x.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage(CinemaValidationMessages.AddressRequired)
            .Length(3, 250)
            .WithMessage(CinemaValidationMessages.AddressLength);
        
        RuleFor(x=>x.City)
            .NotNull()
            .NotEmpty()
            .WithMessage(CinemaValidationMessages.CityRequired)
            .Length(3, 50)
            .WithMessage(CinemaValidationMessages.CityLength);
        
        RuleFor(x=>x.Country)
            .NotNull()
            .NotEmpty()
            .WithMessage(CinemaValidationMessages.CountryRequired)
            .Length(3, 50)
            .WithMessage(CinemaValidationMessages.CountryLength);
    }
}