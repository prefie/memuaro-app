namespace Rememory.WebApi.Dtos.Question;

public class AddQuestionRequestDto
{
    public Guid UserId { get; set; }
    public string? Title { get; set; }
}