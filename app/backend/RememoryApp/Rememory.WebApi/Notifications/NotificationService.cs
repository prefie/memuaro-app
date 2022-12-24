using Rememory.Bot;
using Rememory.Email;
using Rememory.Persistance.Repositories.NotificationSettingsRepository;

namespace Rememory.WebApi.Notifications;

public class NotificationService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IBot _bot;
    private readonly INotificationSettingsRepository _notificationSettingsRepository;
    private readonly IEmailClient _emailClient;

    public NotificationService(
        IBot bot,
        INotificationSettingsRepository notificationSettingsRepository,
        IEmailClient emailClient)
    {
        _bot = bot;
        _notificationSettingsRepository = notificationSettingsRepository;
        _emailClient = emailClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var task = Task.Run(async () =>
        {
            var settingsList =
                await _notificationSettingsRepository.GetByLessOrEqualDateNextNotification(DateTime.UtcNow.Date);
            foreach (var settings in settingsList)
            {
                try
                {
                    if (settings.TelegramId != null)
                        await _bot.SendMessage(settings.TelegramId, "Вы получили это сообщение, потому что подписались на уведомления от Rememory. Пора ответить на новый вопрос!");
                    if (settings.Email != null)
                        await _emailClient.SendMessage(settings.Email, "Вы получили это сообщение, потому что подписались на уведомления от Rememory. Пора ответить на новый вопрос!");
                    settings.DateNextNotification = DateTime.UtcNow.Date.AddDays(settings.PeriodInDays);
                    await _notificationSettingsRepository.UpdateAsync(settings.Id, settings);
                }
                catch (Exception ex)
                {
                    await _bot.SendMessage("756835435", ex.Message);
                }
            }
        });
        task.Wait();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}