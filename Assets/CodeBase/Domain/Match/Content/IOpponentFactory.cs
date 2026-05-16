using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Character.CatSkin.Content;
using CodeBase.Domain.Match.Data;

namespace CodeBase.Domain.Match.Content
{
    public interface IOpponentFactory
    {
        MatchPlayerInfo Create(PlayerId opponentId, CatSkinId localPlayerSkinId);
    }
}