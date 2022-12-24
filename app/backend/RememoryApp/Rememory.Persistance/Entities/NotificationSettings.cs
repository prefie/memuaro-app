namespace Rememory.Persistance.Entities;

public class NotificationSettings : IDatabaseEntity
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? TelegramName { get; set; }
    public string? TelegramId { get; set; }
    public int PeriodInDays { get; set; }
    public DateTime? DateNextNotification { get; set; }
}