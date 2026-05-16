using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class PlayerSaveMenu
    {
        private const string PlayerDataFileName = "PlayerSave.json";

        [MenuItem("Tools/Delete Player Save")]
        public static void DeletePlayerSave()
        {
            string filePath = Path.Combine(Application.persistentDataPath, PlayerDataFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"Deleted: {filePath}");
            }
            else
            {
                Debug.Log($"Not found: {filePath}");
            }
        }
    }
}