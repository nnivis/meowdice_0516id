using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Character.CatSkin.Content;
using CodeBase.Domain.Match.Data;
using Random = UnityEngine.Random;

namespace CodeBase.Domain.Match.Content
{
    public class OpponentFactory : IOpponentFactory
    {
        private readonly string[] _names =
        {
            "Milo",
            "Luna",
            "Coco",
            "Simba",
            "Nala",
            "Leo",
            "Mochi",
            "Oreo"
        };

        public MatchPlayerInfo Create(PlayerId opponentId, CatSkinId localPlayerSkinId)
        {
            string opponentName = CreateOpponentName();
            CatSkinId opponentSkinId = CreateOpponentSkin(localPlayerSkinId);

            return new MatchPlayerInfo(opponentId, opponentName, opponentSkinId);
        }

        private string CreateOpponentName()
        {
            if (_names == null || _names.Length == 0)
                throw new InvalidOperationException("Opponent names are not configured.");

            int randomIndex = Random.Range(0, _names.Length);
            return _names[randomIndex];
        }

        private CatSkinId CreateOpponentSkin(CatSkinId localPlayerSkinId)
        {
            Array allSkinValues = Enum.GetValues(typeof(CatSkinId));
            var availableSkins = new List<CatSkinId>();

            foreach (CatSkinId skinId in allSkinValues)
            {
                if (skinId == CatSkinId.None)
                    continue;

                if (skinId == localPlayerSkinId)
                    continue;

                availableSkins.Add(skinId);
            }

            if (availableSkins.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Could not create opponent skin. No available skins except {localPlayerSkinId}.");
            }

            int randomIndex = Random.Range(0, availableSkins.Count);
            return availableSkins[randomIndex];
        }
    }
}