﻿using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransactionsSummary.Queries.GetTransactionsSummary;

public class GetTransactionsSummaryQuery : IRequest<TransactionsSummaryDto>
{
    public TransactionType TransactionType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}

public class GetTransactionsSummaryQueryHandler : IRequestHandler<GetTransactionsSummaryQuery, TransactionsSummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetTransactionsSummaryQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<TransactionsSummaryDto> Handle(GetTransactionsSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var itemsQuery = _context.Transaction
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

        var items = await itemsQuery
            .GroupBy(x => x.Category.Name)
            .OrderBy(g => g.Key)
            .Skip(offset)
            .Take(limit)
            .Select(group => new TransactionsSummaryItemDto
            {
                CategoryName = group.Key,
                Sum = group.Sum(x => x.Amount)
            })
            .ToListAsync(cancellationToken);

        int totalCount = await itemsQuery.GroupBy(x => x.Category.Name).CountAsync(cancellationToken);

        return new TransactionsSummaryDto
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
