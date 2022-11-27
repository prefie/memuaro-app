using Memuaro.Persistance.Entities;

namespace Memuaro.WebApi.Dtos.Question;

public class GlobalQuestionDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid CategoryId { get; set; }

    public GlobalQuestionDto(GlobalQuestion globalQuestion)
    {
        Id = globalQuestion.Id;
        Title = globalQuestion.Title;
        CategoryId = globalQuestion.CategoryId;
    }
}