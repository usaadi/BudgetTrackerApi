namespace Application.Features.Category.Queries.GetCategories;

using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetCategoriesQuery : IRequest<CategoriesDto>
{
    public CategoryType CategoryType { get; set; }
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoriesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CategoriesDto> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));

        var items = await _context.Category
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId.Value && x.CategoryType == request.CategoryType)
            .OrderBy(x => x.Name)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new CategoriesDto
        {
            Items = items
        };
    }
}
