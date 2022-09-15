namespace Application.Features.Transaction.Commands.UpdateTransaction;

using Application.Common.Interfaces;
using Application.Features.Category.Commands.CreateCategory;
using Application.Features.Transaction.Queries.GetTransactions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateTransactionCommand : IRequest<TransactionDto>
{
    public Guid UniqueId { get; set; }
    public Guid? CategoryUniqueId { get; set; }
    public CreateCategoryCommand? CreateCategoryCommand { get; set; }
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

        var entity = await _context.Transaction
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == request.UniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

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

        entity.Description = request.Description;
        entity.CategoryId = categoryId.Value;
        entity.Amount = request.Amount;
        entity.TransactionDate = request.TransactionDate;

        _context.Transaction.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TransactionDto>(entity);
    }
}