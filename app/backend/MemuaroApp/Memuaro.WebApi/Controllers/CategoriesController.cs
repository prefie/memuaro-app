using Memuaro.Auth;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.CategoryRepository;
using Memuaro.WebApi.Dtos.Categories;
using Memuaro.WebApi.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Memuaro.WebApi.Controllers;

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
}