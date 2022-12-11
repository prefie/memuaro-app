using Rememory.Persistance.Models;

namespace Rememory.WebApi.Dtos.Question;

public class AnswerRequestDto
{
    public string? Answer { get; set; }
    public Status? NewStatus { get; set; }
}