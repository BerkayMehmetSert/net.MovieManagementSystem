using Application.Contracts.Constants.Director;
using Application.Contracts.Requests.Director;
using FluentValidation;

namespace Application.Contracts.Validations.Director;

public class CreateDirectorRequestValidator : AbstractValidator<CreateDirectorRequest>
{
    public CreateDirectorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage(DirectorValidationMessages.FirstNameRequired)
            .Length(3, 50)
            .WithMessage(DirectorValidationMessages.FirstNameLength);
            
        RuleFor(x=>x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage(DirectorValidationMessages.LastNameRequired)
            .Length(3, 50)
            .WithMessage(DirectorValidationMessages.LastNameLength);
        
        RuleFor(x=>x.Nationality)
            .NotNull()
            .NotEmpty()
            .WithMessage(DirectorValidationMessages.NationalityRequired)
            .Length(3, 100)
            .WithMessage(DirectorValidationMessages.NationalityLength);
        
        RuleFor(x=>x.DateOfBirth)
            .NotNull()
            .NotEmpty()
            .WithMessage(DirectorValidationMessages.DateOfBirthRequired);
    }
}