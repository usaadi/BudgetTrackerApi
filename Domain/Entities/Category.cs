namespace Domain.Entities;

using Domain.Common;
using Domain.Enums;

public class Category : BaseEntity, IUserSpecific
{
    public Category(CategoryType categoryType, string name, Guid userUniqueId, string? description = null)
    {
        CategoryType = categoryType;
        Name = name;
        UserUniqueId = userUniqueId;
        Description = description;
    }

    public CategoryType CategoryType { get; set; }
    public string Name { get; set; }
    public Guid UserUniqueId { get; set; }
    public string? Description { get; set; }
}
