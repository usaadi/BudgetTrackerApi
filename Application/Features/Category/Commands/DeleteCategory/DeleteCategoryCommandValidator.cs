using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Category.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCategoryCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;

        RuleFor(v => v.AllowDeleteRelatedData)
            .MustAsync(AllowDeleteRelatedData).WithMessage("There are related transactions for this category.");
    }

    public async Task<bool> AllowDeleteRelatedData(DeleteCategoryCommand command, bool allowDeleteRelatedData, CancellationToken cancellationToken)
    {
        if (allowDeleteRelatedData)
        {
            return true;
        }

        if (_currentUserService.UserUniqueId == null || _currentUserService.UserUniqueId == default)
        {
            throw new Exception("UserUniqueId is empty");
        }

        var category = await _context.Category
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId && x.UniqueId == command.UniqueId)
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(category, nameof(category));

        var transactionsCount = await _context.Transaction.Where(x => x.CategoryId == category.Id).CountAsync(cancellationToken);

        if (transactionsCount > 0)
        {
            throw new RelatedDataExistException();
        }

        return true;
    }
}
