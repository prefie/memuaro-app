using Rememory.Auth;
using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.CategoryRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rememory.WebApi.Dtos.Categories;
using Rememory.WebApi.Exceptions;

namespace Rememory.WebApi.Controllers;

[Authorize]
public class CategoriesController : BaseController
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(
        AuthProvider authProvider,
        ICategoryRepository categoryRepository) : base(authProvider)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<CategoriesDto>> GetAllCategories()
    {
        var result = await _categoryRepository.GetAsync();
        var categories = result
            .Select(category => new CategoryDto(category))
            .ToList();

        return Ok(new CategoriesDto {Categories = categories});
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var result = await _categoryRepository.GetAsync(id);

        if (result == null)
            throw new NotFoundException(nameof(Category), id);

        return Ok(new CategoryDto(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("new")]
    public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] AddCategoryRequestDto request)
    {
        var category = new Category {Id = Guid.NewGuid(), Name = request.Name};

        await _categoryRepository.CreateAsync(category);

        return Ok(category);
    }
}