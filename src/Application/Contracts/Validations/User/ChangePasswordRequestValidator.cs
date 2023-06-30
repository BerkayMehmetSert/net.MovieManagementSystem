using Application.Contracts.Constants.User;
using Application.Contracts.Requests.User;
using FluentValidation;

namespace Application.Contracts.Validations.User;

public class ChangePasswordRequestValidation : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidation()
    {
        RuleFor(x=>x.OldPassword)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired);
        
        RuleFor(x=>x.NewPassword)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired);
        
        RuleFor(x=>x.ConfirmPassword)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired);
    }
}