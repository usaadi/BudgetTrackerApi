using Application.Common.Interfaces;
using Domain.Enums;
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
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");

        RuleFor(v => v.Description)
            .MaximumLength(50).WithMessage("Description must not exceed 50 characters.");

        //RuleFor(v => v.TransactionType)
        //    .MustAsync(NotExceedLimit).WithMessage("You have exceeded limit of created categories. Please consider upgrading to add more.");
    }

    public async Task<bool> BeUniqueName(CreateCategoryCommand command, string name, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserUniqueId == null || _currentUserService.UserUniqueId == default)
        {
            throw new Exception("UserUniqueId is empty");
        }

        return await _context.Category
            .AllAsync(x => x.Name.ToLower() != name.ToLower() || x.TransactionTypeLookupId != (int)command.TransactionType
            || x.UserUniqueId != _currentUserService.UserUniqueId, cancellationToken);
    }

    //public async Task<bool> NotExceedLimit(CreateCategoryCommand command, TransactionType transactionType, CancellationToken cancellationToken)
    //{
    //    const int defaultMaximumCategories = 50;

    //    _ = int.TryParse(System.Environment.GetEnvironmentVariable("MAX_CATEGORIES_FREE"), out int maxCategories);

    //    if (maxCategories <= 0)
    //    {
    //        maxCategories = defaultMaximumCategories;
    //    }

    //    var count = await _context.Category
    //            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId)
    //            .CountAsync(cancellationToken);

    //    return count < maxCategories;
    //}
}
