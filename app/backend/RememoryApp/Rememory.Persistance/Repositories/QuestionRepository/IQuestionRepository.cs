using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.QuestionRepository;

public interface IQuestionRepository : IRepository<Question>
{
    public Task<List<Question>> GetForUserAsync(Guid userId);
}