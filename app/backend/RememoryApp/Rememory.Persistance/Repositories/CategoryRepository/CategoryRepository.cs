using Rememory.Persistance.Client;
using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.CategoryRepository;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private const string CollectionName = "categories";
    
    public CategoryRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }
}