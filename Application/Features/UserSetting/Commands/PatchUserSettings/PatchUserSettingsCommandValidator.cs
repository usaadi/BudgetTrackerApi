using FluentValidation;

namespace Application.Features.UserSetting.Commands.PatchUserSettings;

public class PatchUserSettingsCommandValidator : AbstractValidator<PatchUserSettingsCommand>
{
    public PatchUserSettingsCommandValidator()
    {
        RuleFor(v => v.CurrencySymbol)
            .MaximumLength(10).WithMessage("Description must not exceed 10 characters.");
    }
}
