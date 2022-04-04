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
    private readonly Guid _userUniqueId;

    public CreateCategoryCommandCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Name, nameof(request.Name));

        var entity = new Category(request.CategoryType, request.Name, _userUniqueId, request.Description);

        _context.Category.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}