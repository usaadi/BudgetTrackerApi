namespace Application.Features.Transaction.Commands.CreateTransaction;

using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateTransactionCommand : IRequest<long>
{
    public decimal Amount { get; set; }
    public Guid CategoryUniqueId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }

}

public class CreateTransactionCommandCommandHandler : IRequestHandler<CreateTransactionCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTransactionCommandCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<long> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        if (request.CategoryUniqueId == Guid.Empty)
        {
            throw new ArgumentException("CategoryUniqueId was empty", nameof(request));
        }

        var category = await _context.Category
            .Where(x => x.UniqueId == request.CategoryUniqueId).FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(category, nameof(category));

        var entity = new Transaction
        {
            Amount = request.Amount,
            CategoryId = category.Id,
            TransactionDate = request.TransactionDate,
            Description = request.Description,
            UserUniqueId = _currentUserService.UserUniqueId.Value
        };

        _context.Transaction.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}