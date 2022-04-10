using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Category.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateCategoryCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
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

    public async Task<bool> BeUniqueName(CreateCategoryCommand command, string name, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserUniqueId == null || _currentUserService.UserUniqueId == default)
        {
            throw new Exception("UserUniqueId is empty");
        }

        return await _context.Category
            .AllAsync(x => x.Name != name || x.TransactionTypeLookupId != (int)command.TransactionType
            || x.UserUniqueId != _currentUserService.UserUniqueId, cancellationToken);
    }
}
