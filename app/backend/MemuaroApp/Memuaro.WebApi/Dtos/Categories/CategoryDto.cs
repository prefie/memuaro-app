using Memuaro.Persistance.Entities;

namespace Memuaro.WebApi.Dtos.Categories;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public CategoryDto(Category category)
    {
        Id = category.Id;
        Name = category.Name;
    } 
}