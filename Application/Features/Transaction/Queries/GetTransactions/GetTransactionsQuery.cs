using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Transaction.Queries.GetTransactions;

public class GetTransactionsQuery : IRequest<TransactionsDto>
{
    public TransactionType TransactionType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string? SortBy { get; set; }
    public bool IsDesc { get; set; }
}

public class GetTransactionQueryHandler : IRequestHandler<GetTransactionsQuery, TransactionsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetTransactionQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<TransactionsDto> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        IQueryable<Domain.Entities.Transaction>? itemsQuery = _context.Transaction
            .Include(x => x.Category)
            .ThenInclude(x => x.TransactionTypeLookup)
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId.Value && x.Category.TransactionTypeLookupId == (int)request.TransactionType);

        if (request.FromDate is not null)
        {
            itemsQuery = itemsQuery.Where(x => x.TransactionDate >= request.FromDate);
        }

        if (request.ToDate is not null)
        {
            itemsQuery = itemsQuery.Where(x => x.TransactionDate <= request.ToDate);
        }

        int limit = (request.PageSize > 0 && request.PageNumber > 0) ? request.PageSize : 0;
        int offset = (request.PageNumber - 1) * limit;

        IQueryable<Domain.Entities.Transaction>? orderedQuery;

        if (!request.IsDesc)
        {
            switch (request.SortBy)
            {
                case "category":
                    orderedQuery = itemsQuery.OrderBy(x => x.Category.Name);
                    break;
                case "description":
                    orderedQuery = itemsQuery.OrderBy(x => x.Description);
                    break;
                case "transactionDate":
                    orderedQuery = itemsQuery.OrderBy(x => x.TransactionDate);
                    break;
                case "amount":
                    orderedQuery = itemsQuery.OrderBy(x => x.Amount);
                    break;
                default:
                    orderedQuery = itemsQuery.OrderByDescending(x => x.TransactionDate);
                    break;
            }
        }
        else
        {
            switch (request.SortBy)
            {
                case "category":
                    orderedQuery = itemsQuery.OrderByDescending(x => x.Category.Name);
                    break;
                case "description":
                    orderedQuery = itemsQuery.OrderByDescending(x => x.Description);
                    break;
                case "transactionDate":
                    orderedQuery = itemsQuery.OrderByDescending(x => x.TransactionDate);
                    break;
                case "amount":
                    orderedQuery = itemsQuery.OrderByDescending(x => x.Amount);
                    break;
                default:
                    orderedQuery = itemsQuery.OrderByDescending(x => x.TransactionDate);
                    break;
            }
        }

        var items = await orderedQuery
            .Skip(offset)
            .Take(limit)
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        int totalCount = await itemsQuery
            .Where(x => !x.IsDeleted && !x.Category.IsDeleted)
            .CountAsync(cancellationToken);

        bool hasMore = totalCount > (request.PageNumber - 1) * request.PageSize + items.Count;

        return new TransactionsDto
        {
            Items = items,
            TotalCount = totalCount,
            NextPageNumber = request.PageNumber + 1,
            HasMore = hasMore
        };
    }
}
