namespace Application.Features.Category.Commands.CreateCategory;

using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

public class CreateCategoryCommand : IRequest<long>
{
    public CategoryType CategoryType { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CreateCategoryCommandCommandHandler : IRequestHandler<CreateCategoryCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateCategoryCommandCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<long> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Name, nameof(request.Name));
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));

        var entity = new Category(request.CategoryType, request.Name, _currentUserService.UserUniqueId.Value, request.Description);

        _context.Category.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}