using System;
using UnityEngine;

namespace CodeBase.Domain.Character.CatSkin.Content
{
    [Serializable]
    public class SpritePartVariant
    {
        [SerializeField] private string _category;
        [SerializeField] private string _label;

        public string Category => _category;
        public string Label => _label;
    }
}