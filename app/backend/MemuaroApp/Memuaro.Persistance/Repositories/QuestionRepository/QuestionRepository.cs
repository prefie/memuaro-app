using Memuaro.Persistance.Client;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;
using MongoDB.Driver;

namespace Memuaro.Persistance.Repositories.QuestionRepository;

public class QuestionRepository: BaseRepository<Question>, IQuestionRepository
{
    private const string CollectionName = "questions";
    
    public QuestionRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }

    public Task<List<Question>> GetForUserAsync(Guid userId) => 
        MongoCollection.Find(entity => entity.UserId == userId).ToListAsync();
}