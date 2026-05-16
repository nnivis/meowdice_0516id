using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Character.CatSkin.Content;

namespace CodeBase.Domain.Match.Data
{
    public class MatchPlayerInfo
    {
        public PlayerId PlayerId { get; }
        public string Name { get; }
        public CatSkinId SkinId { get; }
        public int FinalScore { get; }
        public bool Winner { get; }

        public MatchPlayerInfo(PlayerId playerId, string name, CatSkinId skinId)
        {
            PlayerId = playerId;
            Name = name;
            SkinId = skinId;
        }

        private MatchPlayerInfo(PlayerId playerId, string name, CatSkinId skinId, int finalScore, bool winner)
        {
            PlayerId = playerId;
            Name = name;
            SkinId = skinId;
            FinalScore = finalScore;
            Winner = winner;
        }

        public MatchPlayerInfo WithResult(int finalScore, bool winner) =>
            new MatchPlayerInfo(PlayerId, Name, SkinId, finalScore, winner);
    }
}
