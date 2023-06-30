using Application.Contracts.Constants.Award;
using Application.Contracts.Requests.Award;
using FluentValidation;

namespace Application.Contracts.Validations.Award;

public class UpdateAwardRequestValidator : AbstractValidator<UpdateAwardRequest>
{
    public UpdateAwardRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(AwardValidationMessages.NameRequired)
            .Length(3, 50)
            .WithMessage(AwardValidationMessages.NameLength);
        
        RuleFor(x=>x.Description)
            .Length(3,250)
            .WithMessage(AwardValidationMessages.DescriptionLength);
        
        RuleFor(x=>x.Date)
            .NotNull()
            .NotEmpty()
            .WithMessage(AwardValidationMessages.DateRequired);
    }
}