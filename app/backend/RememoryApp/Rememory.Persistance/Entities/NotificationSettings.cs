namespace Rememory.Persistance.Entities;

public class NotificationSettings : IDatabaseEntity
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? TelegramName { get; set; }
    public int PeriodInDays { get; set; }
}