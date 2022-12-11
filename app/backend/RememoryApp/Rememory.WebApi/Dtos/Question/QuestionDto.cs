namespace Rememory.WebApi.Dtos.Question;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid UserId { get; set; }
    public string? Answer { get; set; }
    public string? Status { get; set; }
    public Guid CategoryId { get; set; }

    public QuestionDto(Rememory.Persistance.Entities.Question question)
    {
        Id = question.Id;
        Title = question.Title;
        UserId = question.UserId;
        Answer = question.Answer;
        Status = question.Status.ToString();
        CategoryId = question.CategoryId;
    }

    public QuestionDto()
    {
    }
}