using Rememory.Persistance.Entities;
using Rememory.Persistance.Models;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.UserSettingsRepository;

public interface IUserSettingsRepository : IRepository<UserSettings>
{
    public Task<UserSettings> SetAddressSettings(Guid userid, AddressSettings addressSettings);
    public Task<AddressSettings?> GetAddressSettings(Guid userId);
}