using CodeBase.Data;

namespace CodeBase.Infrastructure.DataProvider
{
    public interface IPersistentData
    {
        public PlayerData PlayerData { get; set; }
    }
}