using Rememory.Persistance.Models;

namespace Rememory.Persistance.Entities;

public class UserSettings : IDatabaseEntity
{
    public Guid Id { get; set; }
    public AddressSettings? AddressSettings { get; set; }
}