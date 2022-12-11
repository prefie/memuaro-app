namespace Rememory.WebApi.Dtos.Question;

public class GetGlobalQuestionsRequestDto
{
    public Guid? UserId { get; set; }
    public Guid[]? CategoryIds { get; set; }
}