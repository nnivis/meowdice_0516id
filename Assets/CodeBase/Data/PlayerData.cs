using System;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Character.CatSkin.Content;
using Newtonsoft.Json;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        [JsonProperty("id")]
        private PlayerId _id;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("score")]
        private int _score;
        
        [JsonProperty("skinId")]
        private CatSkinId _skinId;

        public PlayerData()
        {
            _id = new PlayerId(1); // или PlayerId.Unknown
            _name = string.Empty;
            _score = 0;
            _skinId = CatSkinId.None;
        }

        [JsonConstructor]
        public PlayerData(PlayerId id, string name, int score, CatSkinId skinId = CatSkinId.None)
        {
            _id = id;
            _name = name ?? string.Empty;
            Score = score;
            _skinId = skinId;
        }

        public PlayerId Id => _id;

        public string Name
        {
            get => _name;
            set => _name = value ?? string.Empty;
        }

        public int Score
        {
            get => _score;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _score = value;
            }
        }
        
        public CatSkinId SkinId
        {
            get => _skinId;
            set => _skinId = value;
        }
    }
}