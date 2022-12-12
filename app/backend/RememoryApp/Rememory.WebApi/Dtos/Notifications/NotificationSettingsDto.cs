using Rememory.Persistance.Entities;

namespace Rememory.WebApi.Dtos.Notifications;

public class NotificationSettingsDto
{
    public string? Email { get; set; }
    public string? TelegramName { get; set; }
    public int PeriodInDays { get; set; }

    public NotificationSettingsDto(NotificationSettings settings)
    {
        Email = settings.Email;
        TelegramName = settings.TelegramName;
        PeriodInDays = settings.PeriodInDays;
    }

    public NotificationSettingsDto()
    {
    }
}