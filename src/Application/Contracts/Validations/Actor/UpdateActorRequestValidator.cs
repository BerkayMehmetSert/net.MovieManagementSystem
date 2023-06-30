using Application.Contracts.Constants.Actor;
using Application.Contracts.Requests.Actor;
using FluentValidation;

namespace Application.Contracts.Validations.Actor;

public class UpdateActorRequestValidator : AbstractValidator<UpdateActorRequest>
{
    public UpdateActorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage(ActorValidationMessages.FirstNameRequired)
            .Length(3, 50)
            .WithMessage(ActorValidationMessages.FirstNameLength);
            
        RuleFor(x=>x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage(ActorValidationMessages.LastNameRequired)
            .Length(3, 50)
            .WithMessage(ActorValidationMessages.LastNameLength);
        
        RuleFor(x=>x.Nationality)
            .NotNull()
            .NotEmpty()
            .WithMessage(ActorValidationMessages.NationalityRequired)
            .Length(3, 100)
            .WithMessage(ActorValidationMessages.NationalityLength);
        
        RuleFor(x=>x.DateOfBirth)
            .NotNull()
            .NotEmpty()
            .WithMessage(ActorValidationMessages.DateOfBirthRequired);
    }
}