using FluentValidation;
using GameStore.Api.DTOs.Auth;

namespace GameStore.Api.Validators.Auth;

public sealed class RefreshTokenRequestValidator
    : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}