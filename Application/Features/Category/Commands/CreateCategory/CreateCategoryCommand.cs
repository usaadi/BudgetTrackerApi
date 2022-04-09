namespace Application.Features.Category.Commands.CreateCategory;

using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

public class CreateCategoryCommand : IRequest<long>
{
    public TransactionType TransactionType { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateCategoryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<long> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Name, nameof(request.Name));
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));

        var entity = new Category
        {
            TransactionTypeLookupId = (int)request.TransactionType,
            Name = request.Name,
            Description = request.Description,
            UserUniqueId = _currentUserService.UserUniqueId.Value
        };

        _context.Category.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}