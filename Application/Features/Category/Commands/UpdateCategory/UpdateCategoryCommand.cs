namespace Application.Features.Category.Commands.UpdateCategory;

using Application.Common.Interfaces;
using Application.Features.Category.Queries.GetCategories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateCategoryCommand : IRequest<CategoryDto>
{
    public Guid UniqueId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Name, nameof(request.Name));
        ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        if (request.UniqueId == default) { throw new ArgumentException("request.UniqueId is empty", nameof(request)); }

        var entity = await _context.Category
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == request.UniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        entity.Name = request.Name;
        entity.Description = request.Description;

        _context.Category.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(entity);
    }
}