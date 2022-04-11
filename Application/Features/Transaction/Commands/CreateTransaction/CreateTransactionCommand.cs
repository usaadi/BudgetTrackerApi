using Application.Common.Interfaces;
using Application.Features.Category.Commands.CreateCategory;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Transaction.Commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<long>
{
    public decimal Amount { get; set; }
    public Guid? CategoryUniqueId { get; set; }
    public CreateCategoryCommand? CreateCategoryCommand { get; set; }
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

        long? categoryId = null;
        if (request.CategoryUniqueId != null)
        {
            var category = await _context.Category
                .Where(x => x.UniqueId == request.CategoryUniqueId).FirstOrDefaultAsync(cancellationToken);

            ArgumentNullException.ThrowIfNull(category, nameof(category));

            categoryId = category.Id;
        }
        else
        {
            var category = new Domain.Entities.Category();

            ArgumentNullException.ThrowIfNull(request.CreateCategoryCommand, nameof(request.CreateCategoryCommand));
            ArgumentNullException.ThrowIfNull(request.CreateCategoryCommand.Name, nameof(request.CreateCategoryCommand.Name));

            category.TransactionTypeLookupId = (int)request.CreateCategoryCommand.TransactionType;
            category.Name = request.CreateCategoryCommand.Name;
            category.Description = request.CreateCategoryCommand.Description;
            category.UserUniqueId = _currentUserService.UserUniqueId.Value;

            _context.Category.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            categoryId = category.Id;
        }

        ArgumentNullException.ThrowIfNull(categoryId, nameof(categoryId));

        var entity = new Domain.Entities.Transaction
        {
            Amount = request.Amount,
            CategoryId = categoryId.Value,
            TransactionDate = request.TransactionDate,
            Description = request.Description,
            UserUniqueId = _currentUserService.UserUniqueId.Value
        };

        _context.Transaction.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}