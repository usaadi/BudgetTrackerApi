using Application.Common.Interfaces;
using Application.Features.Category.Queries.GetCategories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Transaction.Commands.DeleteTransaction;

public class DeleteTransactionCommand : IRequest
{
    public Guid UniqueId { get; set; }
}

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTransactionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        if (request.UniqueId == Guid.Empty)
        {
            throw new ArgumentException("uniqueId in delete category command was empty", nameof(request));
        }

        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));

        var entity = await _context.Transaction
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == request.UniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        entity.IsDeleted = true;

        _context.Transaction.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}