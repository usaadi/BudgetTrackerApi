using FluentValidation;

namespace Application.Features.UserSetting.Commands.SetUserSettings;

public class SetUserSettingsCommandValidator : AbstractValidator<SetUserSettingsCommand>
{
    public SetUserSettingsCommandValidator()
    {
        RuleFor(v => v.CurrencySymbol)
            .MaximumLength(10).WithMessage("Description must not exceed 10 characters.");
    }
}
