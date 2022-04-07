using Application.Common.Interfaces;
using Application.Features.Category.Queries.GetCategories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Category.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public Guid UniqueId { get; set; }
    public bool AllowDeleteRelatedData { get; set; }
}

public class DeleteCategoryCommandCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCategoryCommandCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.UniqueId == Guid.Empty)
        {
            throw new ArgumentException("uniqueId in delete category command was empty", nameof(request));
        }

        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));

        var entity = await _context.Category
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == request.UniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        entity.IsDeleted = true;

        _context.Category.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}