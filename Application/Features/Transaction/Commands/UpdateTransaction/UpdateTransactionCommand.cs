namespace Application.Features.Transaction.Commands.UpdateTransaction;

using Application.Common.Interfaces;
using Application.Features.Transaction.Queries.GetTransactions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateTransactionCommand : IRequest<TransactionDto>
{
    public Guid UniqueId { get; set; }
    public Guid CategoryUniqueId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
}

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateTransactionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        if (request.UniqueId == default) { throw new ArgumentException("request.UniqueId is empty", nameof(request)); }
        if (request.CategoryUniqueId == default) { throw new ArgumentException("request.CategoryUniqueId is empty", nameof(request)); }

        var entity = await _context.Transaction
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == request.UniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var category = await _context.Category
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == request.CategoryUniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(category, nameof(category));

        entity.Description = request.Description;
        entity.CategoryId = category.Id;
        entity.Amount = request.Amount;
        entity.TransactionDate = request.TransactionDate;

        _context.Transaction.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TransactionDto>(entity);
    }
}