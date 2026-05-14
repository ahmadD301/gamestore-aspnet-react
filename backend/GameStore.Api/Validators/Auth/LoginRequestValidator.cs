using FluentValidation;
using GameStore.Api.DTOs.Auth;

namespace GameStore.Api.Validators.Auth;

public sealed class LoginRequestValidator
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}