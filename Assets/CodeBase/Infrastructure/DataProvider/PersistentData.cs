using CodeBase.Data;

namespace CodeBase.Infrastructure.DataProvider
{
    public class PersistentData : IPersistentData
    {
        public PlayerData PlayerData { get; set; }
    }
}