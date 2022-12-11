using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.GlobalQuestionRepository;

public interface IGlobalQuestionRepository : IRepository<GlobalQuestion>
{
    public Task<GlobalQuestion> GetRandomGlobalQuestionWithExcept(HashSet<Guid> exceptIds);

    public Task<List<GlobalQuestion>>
        GetGlobalQuestionWithExcept(HashSet<Guid>? exceptIds, HashSet<Guid>? exceptCategoriesIds);
}