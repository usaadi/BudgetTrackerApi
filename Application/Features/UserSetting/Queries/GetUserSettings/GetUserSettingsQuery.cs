using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserSetting.Queries.GetUserSettings;

public class GetUserSettingsQuery : IRequest<UserSettingsDto>
{
}

public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, UserSettingsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetUserSettingsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<UserSettingsDto> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var userSetting = await _context.UserSetting
            .FirstOrDefaultAsync(x => x.UserUniqueId == _currentUserService.UserUniqueId.Value);

        if (userSetting is not null)
        {
            return new UserSettingsDto
            {
                CurrencySymbol = userSetting.CurrencySymbol
            };
        }
        else
        {
            return new UserSettingsDto();
        }
    }
}
