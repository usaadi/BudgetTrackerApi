using Application.Common.Interfaces;
using Application.Features.UserSetting.Queries.GetUserSettings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserSetting.Commands.SetUserSettings;

public class SetUserSettingsCommand : IRequest<UserSettingsDto>
{
    public string CurrencySymbol { get; set; } = "";
}

public class SetUserSettingsCommandHandler : IRequestHandler<SetUserSettingsCommand, UserSettingsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SetUserSettingsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UserSettingsDto> Handle(SetUserSettingsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));

        var userSetting = await _context.UserSetting
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userSetting is null)
        {
            userSetting = new Domain.Entities.UserSetting
            {
                CurrencySymbol = request.CurrencySymbol,
                UserUniqueId = _currentUserService.UserUniqueId.Value
            };

            _context.UserSetting.Add(userSetting);
        }
        else
        {
            userSetting.CurrencySymbol = request.CurrencySymbol;
            _context.UserSetting.Update(userSetting);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UserSettingsDto
        {
            CurrencySymbol = userSetting.CurrencySymbol
        };
    }
}