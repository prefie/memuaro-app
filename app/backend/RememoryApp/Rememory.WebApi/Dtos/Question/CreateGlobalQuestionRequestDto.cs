namespace Rememory.WebApi.Dtos.Question;

public class CreateGlobalQuestionRequestDto
{
    public string? Title { get; set; }
    public Guid? CategoryId { get; set; }
}