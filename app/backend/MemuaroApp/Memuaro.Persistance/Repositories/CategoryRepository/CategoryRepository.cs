using Memuaro.Persistance.Client;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;

namespace Memuaro.Persistance.Repositories.CategoryRepository;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private const string CollectionName = "categories";
    
    public CategoryRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }
}