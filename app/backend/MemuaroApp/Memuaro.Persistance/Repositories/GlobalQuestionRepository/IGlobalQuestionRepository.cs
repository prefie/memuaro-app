using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;

namespace Memuaro.Persistance.Repositories.GlobalQuestionRepository;

public interface IGlobalQuestionRepository : IRepository<GlobalQuestion>
{
    public Task<GlobalQuestion> GetRandomGlobalQuestionWithExcept(HashSet<Guid> exceptIds);

    public Task<List<GlobalQuestion>>
        GetGlobalQuestionWithExcept(HashSet<Guid>? exceptIds, HashSet<Guid>? exceptCategoriesIds);
}