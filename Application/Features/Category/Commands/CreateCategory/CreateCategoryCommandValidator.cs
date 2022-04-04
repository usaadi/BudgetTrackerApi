using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Category.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");

        RuleFor(v => v.Description)
            .MaximumLength(100).WithMessage("Description must not exceed 50 characters.");
    }

    public async Task<bool> BeUniqueName(CreateCategoryCommand command, string name, CancellationToken cancellationToken)
    {
        return await _context.Category
            .AllAsync(l => l.Name != name || command.CategoryType != l.CategoryType, cancellationToken);
    }
}
