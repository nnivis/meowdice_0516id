using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Character.CatSkin.Content;

namespace CodeBase.Infrastructure.DataProvider
{
    public interface ILocalPlayerDataProvider
    {
        PlayerId GetLocalPlayerId();
        CatSkinId GetLocalCatSkinId();
    }
}