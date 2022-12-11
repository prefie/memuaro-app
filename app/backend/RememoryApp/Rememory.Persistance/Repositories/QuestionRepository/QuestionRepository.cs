using MongoDB.Driver;
using Rememory.Persistance.Client;
using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.QuestionRepository;

public class QuestionRepository: BaseRepository<Question>, IQuestionRepository
{
    private const string CollectionName = "questions";
    
    public QuestionRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }

    public Task<List<Question>> GetForUserAsync(Guid userId) => 
        MongoCollection.Find(entity => entity.UserId == userId).ToListAsync();
}