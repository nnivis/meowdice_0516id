using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.Domain.Character.CatSkin.Content;
using CodeBase.Infrastructure.DataProvider;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure
{
    public class DataBootstrap : IDataBootstrap
    {
        private const string DefaultPlayerName = "Murchik";
        private const CatSkinId DefaultPlayerSkin = CatSkinId.White;

        private readonly IDataProvider _dataProvider;
        private readonly IPersistentData _persistentData;

        public DataBootstrap(IDataProvider dataProvider, IPersistentData persistentData)
        {
            _persistentData = persistentData;
            _dataProvider = dataProvider;
        }

        public void Initialize() => EnsureDefaults();

        private void EnsureDefaults()
        {
            if (_dataProvider.TryLoad() == false)
            {
                _persistentData.PlayerData = CreateNewPlayerData();
                _dataProvider.Save();
                return;
            }

            if (EnsureValidData(_persistentData.PlayerData))
                _dataProvider.Save();
        }

        private PlayerData CreateNewPlayerData()
        {
            PlayerData playerData = new PlayerData();
            ApplyDefaults(playerData);
            return playerData;
        }

        private bool EnsureValidData(PlayerData playerData)
        {
            if (playerData == null)
            {
                _persistentData.PlayerData = CreateNewPlayerData();
                return true;
            }

            return ApplyDefaults(playerData);
        }

        private bool ApplyDefaults(PlayerData playerData)
        {
            bool changed = false;

            if (playerData.SkinId == CatSkinId.None)
            {
                playerData.SkinId = DefaultPlayerSkin;
                changed = true;
            }

            if (string.IsNullOrEmpty(playerData.Name))
            {
                playerData.Name = DefaultPlayerName;
                changed = true;
            }

            return changed;
        }
        
    }
}