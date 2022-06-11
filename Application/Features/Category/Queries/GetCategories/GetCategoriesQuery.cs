namespace Application.Features.Category.Queries.GetCategories;

using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetCategoriesQuery : IRequest<CategoriesDto>
{
    public TransactionType TransactionType { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
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

        int limit = (request.PageSize > 0 && request.PageNumber > 0) ? request.PageSize : 0;
        int offset = (request.PageNumber - 1) * limit;

        var itemsQuery = _context.Category
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId.Value &&
            x.TransactionTypeLookupId == (int)request.TransactionType);

        var items = await itemsQuery
            .OrderBy(x => x.Name)
            .Skip(offset)
            .Take(limit)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        int totalCount = await itemsQuery
            .Where(x => !x.IsDeleted)
            .CountAsync(cancellationToken);

        return new CategoriesDto
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
