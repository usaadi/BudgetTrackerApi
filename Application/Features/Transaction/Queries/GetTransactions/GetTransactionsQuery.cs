﻿using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Transaction.Queries.GetTransactions;

public class GetTransactionsQuery : IRequest<TransactionsDto>
{
    public TransactionType TransactionType { get; set; }
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

        var items = await _context.Transaction
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId.Value && x.TransactionTypeLookupId == (int)request.TransactionType)
            .OrderByDescending(x => x.TransactionDate)
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new TransactionsDto
        {
            Items = items
        };
    }
}