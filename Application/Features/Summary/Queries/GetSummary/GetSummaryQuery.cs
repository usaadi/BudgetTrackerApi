using Application.Common.Interfaces;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Summary.Queries.GetSummary;

public class GetSummaryQuery : IRequest<SummaryDto>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class GetSummaryQueryHandler : IRequestHandler<GetSummaryQuery, SummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetSummaryQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<SummaryDto> Handle(GetSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var itemsQuery = _context.Transaction
            .Include(x => x.Category)
            .ThenInclude(x => x.TransactionTypeLookup)
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId.Value);

        if (request.FromDate is not null)
        {
            itemsQuery = itemsQuery.Where(x => x.TransactionDate >= request.FromDate);
        }

        if (request.ToDate is not null)
        {
            itemsQuery = itemsQuery.Where(x => x.TransactionDate <= request.ToDate);
        }

        var items = await itemsQuery
            .GroupBy(x => x.Category.TransactionTypeLookupId)
            .Select(group => new
            {
                TransactionTypeLookupId = group.Key,
                Sum = group.Sum(x => x.Amount)
            })
            .ToListAsync(cancellationToken);

        decimal expensesSum = items.Where(x => x.TransactionTypeLookupId == (int)TransactionType.Expenses).Select(x => x.Sum).FirstOrDefault();
        decimal incomeSum = items.Where(x => x.TransactionTypeLookupId == (int)TransactionType.Income).Select(x => x.Sum).FirstOrDefault();
        decimal balance = incomeSum - expensesSum;

        return new SummaryDto
        {
            ExpensesSum = expensesSum,
            IncomeSum = incomeSum,
            Balance = balance
        };
    }
}
