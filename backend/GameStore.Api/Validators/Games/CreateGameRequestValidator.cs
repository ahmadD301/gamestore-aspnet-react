using FluentValidation;
using GameStore.Api.DTOs.Games;

namespace GameStore.Api.Validators.Games;

public sealed class CreateGameRequestValidator
    : AbstractValidator<CreateGameRequestDto>
{
    public CreateGameRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CoverImageUrl)
            .MaximumLength(2000000);

        RuleFor(x => x.GenreId)
            .NotEmpty();
    }
}