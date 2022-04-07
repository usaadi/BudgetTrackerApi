using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Category.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCategoryCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");

        RuleFor(v => v.Description)
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
    }

    public async Task<bool> BeUniqueName(UpdateCategoryCommand command, string name, CancellationToken cancellationToken)
    {
        var category = await _context.Category
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == command.uniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(category, nameof(category));

        return await _context.Category
            .AllAsync(x => x.Name != name || x.TransactionTypeLookupId != category.TransactionTypeLookupId || x.UniqueId == command.uniqueId, cancellationToken);
    }
}
