using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Character.CatSkin.Content;

namespace CodeBase.Infrastructure.DataProvider
{
    public class PersistentLocalPlayerDataProvider : ILocalPlayerDataProvider
    {
        private readonly IPersistentData _persistent;

        public PersistentLocalPlayerDataProvider(IPersistentData persistent) => _persistent = persistent;

        public PlayerId GetLocalPlayerId() => _persistent.PlayerData.Id;
        
         public CatSkinId GetLocalCatSkinId() => _persistent.PlayerData.SkinId;
    }
}