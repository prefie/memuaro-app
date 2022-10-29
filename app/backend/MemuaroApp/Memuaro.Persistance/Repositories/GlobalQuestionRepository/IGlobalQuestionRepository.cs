using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;

namespace Memuaro.Persistance.Repositories.GlobalQuestionRepository;

public interface IGlobalQuestionRepository : IRepository<GlobalQuestion>
{
    public Task<GlobalQuestion> GetRandomGlobalQuestionForUser(Guid userId);
}