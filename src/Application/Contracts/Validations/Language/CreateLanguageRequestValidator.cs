﻿using Application.Contracts.Constants.Language;
using Application.Contracts.Requests.Language;
using FluentValidation;

namespace Application.Contracts.Validations.Language;

public class CreateLanguageRequestValidator : AbstractValidator<CreateLanguageRequest>
{
    public CreateLanguageRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(LanguageValidationMessages.NameRequired)
            .Length(3, 50)
            .WithMessage(LanguageValidationMessages.NameLength);
        
        RuleFor(x=>x.Description)
            .Length(3,250)
            .WithMessage(LanguageValidationMessages.DescriptionLength);
    }
}