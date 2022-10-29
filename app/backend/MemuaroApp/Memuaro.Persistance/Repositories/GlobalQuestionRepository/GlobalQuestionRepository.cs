using Memuaro.Persistance.Client;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;
using MongoDB.Driver;

namespace Memuaro.Persistance.Repositories.GlobalQuestionRepository;

public class GlobalQuestionRepository : BaseRepository<GlobalQuestion>, IGlobalQuestionRepository
{
    private const string CollectionName = "globalQuestions";
    
    public GlobalQuestionRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }

    public Task<GlobalQuestion> GetRandomGlobalQuestionForUser(Guid userId) =>
        MongoCollection.Find(globalQuestion => globalQuestion.UserIds != null && !globalQuestion.UserIds.Contains(userId)).FirstOrDefaultAsync();
}