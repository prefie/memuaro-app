namespace Memuaro.Persistance.Entities;

public class Category : IDatabaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}