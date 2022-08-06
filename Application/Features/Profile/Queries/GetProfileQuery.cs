using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Profile.Queries
{
    public class GetProfileQuery : IRequest<ProfileDto>
    {

    }

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetProfileQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var result = new ProfileDto
            {
                FullName = _currentUserService.FullName,
                Email = _currentUserService.Email
            };

            return await Task.FromResult(result);
        }
    }
}
