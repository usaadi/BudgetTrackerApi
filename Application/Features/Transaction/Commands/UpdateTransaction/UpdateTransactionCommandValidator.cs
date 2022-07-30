using Application.Features.Category.Commands.CreateCategory;
using FluentValidation;

namespace Application.Features.Transaction.Commands.UpdateTransaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator(IValidator<CreateCategoryCommand> createCategoryCommandValidator)
    {
        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.CreateCategoryCommand!)
            .SetValidator(createCategoryCommandValidator);
    }
}
