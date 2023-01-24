using Rememory.Persistance.Client;
using Rememory.Persistance.Entities;
using Rememory.Persistance.Models;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.UserSettingsRepository;

public class UserSettingsRepository : BaseRepository<UserSettings>, IUserSettingsRepository
{
    private const string CollectionName = "userSettings";

    public UserSettingsRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }

    public async Task<UserSettings> SetAddressSettings(Guid userid, AddressSettings addressSettings)
    {
        var newSettings = new UserSettings
        {
            Id = userid,
            AddressSettings = addressSettings,
        };
        var currSettings = await GetAsync(userid);
        if (currSettings == null)
            await CreateAsync(newSettings);
        else
            await UpdateAsync(userid, newSettings);

        return newSettings;
    }

    public async Task<AddressSettings?> GetAddressSettings(Guid userId)
    {
        var userSettings = await GetAsync(userId);
        return userSettings?.AddressSettings ?? null;
    }
}