using Rememory.Persistance.Client;
using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.NotificationSettingsRepository;

public class NotificationSettingsRepository : BaseRepository<NotificationSettings>, INotificationSettingsRepository
{
    private const string CollectionName = "notificationSettings";
    
    public NotificationSettingsRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }

    public async Task<NotificationSettings> CreateOrUpdate(NotificationSettings settings)
    {
        var currSettings = await GetAsync(settings.Id);
        if (currSettings == null)
            await CreateAsync(settings);
        else
            await UpdateAsync(settings.Id, settings);

        return settings;
    }
}