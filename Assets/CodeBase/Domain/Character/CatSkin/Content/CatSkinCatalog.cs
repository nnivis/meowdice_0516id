using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Domain.Character.CatSkin.Content
{
    [CreateAssetMenu(fileName = "CatSkinCatalog", menuName = "Character/Cat Skin Catalog")]
    public class CatSkinCatalog : ScriptableObject
    {
        [SerializeField] private CatSkinDefinition[] _skins;

        private Dictionary<CatSkinId, CatSkinDefinition> _map;

        public CatSkinDefinition Get(CatSkinId skinId)
        {
            EnsureInitialized();

            if (_map.TryGetValue(skinId, out var skin))
                return skin;

            throw new InvalidOperationException(
                $"Skin '{skinId}' is not registered in catalog '{name}'.");
        }

        private void EnsureInitialized()
        {
            if (_map != null)
                return;

            _map = new Dictionary<CatSkinId, CatSkinDefinition>();

            foreach (var skin in _skins)
            {
                if (skin == null)
                    continue;

                _map[skin.SkinId] = skin;
            }
        }
    }
}