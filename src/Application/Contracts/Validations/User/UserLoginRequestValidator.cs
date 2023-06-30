using Application.Contracts.Constants.User;
using Application.Contracts.Requests.User;
using FluentValidation;

namespace Application.Contracts.Validations.User;

public class UserLoginRequestValidation : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidation()
    {
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