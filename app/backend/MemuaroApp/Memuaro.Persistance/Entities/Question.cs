namespace Memuaro.Persistance.Entities;

public class Question : IDatabaseEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid UserId { get; set; }
    public string? Answer { get; set; }
}