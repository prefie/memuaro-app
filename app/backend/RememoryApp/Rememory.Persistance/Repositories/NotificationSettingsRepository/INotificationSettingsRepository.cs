using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.NotificationSettingsRepository;

public interface INotificationSettingsRepository : IRepository<NotificationSettings>
{
    public Task<NotificationSettings> CreateOrUpdate(NotificationSettings settings);
}