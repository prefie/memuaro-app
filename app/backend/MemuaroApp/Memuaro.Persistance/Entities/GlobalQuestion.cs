namespace Memuaro.Persistance.Entities;

public class GlobalQuestion : IDatabaseEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public List<Guid>? UserIds { get; set; }
}