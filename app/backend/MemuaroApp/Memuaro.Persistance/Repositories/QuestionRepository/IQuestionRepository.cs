using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;

namespace Memuaro.Persistance.Repositories.QuestionRepository;

public interface IQuestionRepository : IRepository<Question>
{
    public Task<List<Question>> GetForUserAsync(Guid userId);
}