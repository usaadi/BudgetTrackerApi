namespace Application.Features.Category.Queries.GetCategories;

using Application.Common;
using Domain.Enums;

public class CategoryDto : BaseDto
{
    public CategoryType CategoryType { get; set; }
    public string Name { get; set; } = null!;
    public Guid UserUniqueId { get; set; }
    public string? Description { get; set; }
}
