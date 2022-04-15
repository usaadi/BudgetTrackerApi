using Application.Features.Category.Commands.CreateCategory;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Transaction.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public const int maximumTransactions = 20;

    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTransactionCommandValidator(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IValidator<CreateCategoryCommand> createCategoryCommandValidator)
    {
        _context = context;
        _currentUserService = currentUserService;

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.CreateCategoryCommand!)
            .SetValidator(createCategoryCommandValidator);

        RuleFor(v => v.TransactionDate)
            .MustAsync(NotExceedLimit).WithMessage("You have exceeded limit of created transactions. Please consider upgrading to add more.");
    }

    public async Task<bool> NotExceedLimit(CreateTransactionCommand command, DateTime transactionDate, CancellationToken cancellationToken)
    {
        const int defaultMaximumTransactions = 20;

        _ = int.TryParse(System.Environment.GetEnvironmentVariable("MAX_TRANSACTIONS_FREE"), out int maxTransactions);

        if (maxTransactions <= 0)
        {
            maxTransactions = defaultMaximumTransactions;
        }

        var count = await _context.Transaction
            .Where(x => x.UserUniqueId == _currentUserService.UserUniqueId)
            .CountAsync();

        return count < maxTransactions;
    }
}
