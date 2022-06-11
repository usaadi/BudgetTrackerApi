namespace Application.Features.Category.Queries.GetCategories;

public class CategoriesDto
{
    public CategoriesDto()
    {
        Items = new List<CategoryDto>();
    }

    public IList<CategoryDto> Items { get; set; }
    public int TotalCount { get; internal set; }
}
