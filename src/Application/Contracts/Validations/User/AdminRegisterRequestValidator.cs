using Application.Contracts.Constants.User;
using Application.Contracts.Requests.User;
using FluentValidation;

namespace Application.Contracts.Validations.User;

public class AdminRegisterRequestValidator : AbstractValidator<AdminRegisterRequest>
{
    public AdminRegisterRequestValidator()
    {
        RuleFor(x=>x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.FirstNameRequired)
            .Length(3, 50)
            .WithMessage(UserValidationMessages.FirstNameLength);
        
        RuleFor(x=>x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.LastNameRequired)
            .Length(3, 50)
            .WithMessage(UserValidationMessages.LastNameLength);
        
        RuleFor(x=>x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.EmailRequired)
            .EmailAddress()
            .WithMessage(UserValidationMessages.EmailInvalid);
        
        RuleFor(x=>x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired);
    }
}