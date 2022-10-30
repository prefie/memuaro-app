using Memuaro.Persistance.Models;

namespace Memuaro.WebApi.Dtos.Question;

public class AnswerRequestDto
{
    public string? Answer { get; set; }
    public Status? NewStatus { get; set; }
}