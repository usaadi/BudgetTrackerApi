namespace Domain.Entities;

using Domain.Common;
using Domain.Enums;

public class Category : BaseEntity, IUserSpecific
{
    public Category(int categoryTypeLookupId, string name, Guid userUniqueId, string? description = null)
    {
        CategoryTypeLookupId = categoryTypeLookupId;
        Name = name;
        UserUniqueId = userUniqueId;
        Description = description;
    }

    public int CategoryTypeLookupId { get; set; }
    public Guid UserUniqueId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public CategoryTypeLookup CategoryTypeLookup { get; set; } = null!;
}
