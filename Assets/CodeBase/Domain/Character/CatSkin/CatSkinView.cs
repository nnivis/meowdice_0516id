using CodeBase.Domain.Character.CatSkin.Content;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace CodeBase.Domain.Character.CatSkin
{
    public class CatSkinView : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private CatSkinCatalog _catalog;

        [Header("Resolvers")]
        [SerializeField] private SpriteResolver _bodyResolver;
        [SerializeField] private SpriteResolver _tailResolver;
        [SerializeField] private SpriteResolver _eyesResolver;
        [SerializeField] private SpriteResolver _accessoryFrontResolver;
        [SerializeField] private SpriteResolver _accessoryBackResolver;
        
        [Header("TMP")]
        [SerializeField] private TextMeshProUGUI _nameText;

        public void Apply(CatSkinId skinId, string name)
        {
            SetName(name);
            
            if (_catalog == null)
            {
                Debug.LogError("CatSkinCatalog is not assigned.", this);
                return;
            }
            
            CatSkinDefinition definition = _catalog.Get(skinId);
            ApplyDefinition(definition);
        }

        public void SetName(string name) => _nameText.text = "~ " + name + " ~";

        public void ApplyDefinition(CatSkinDefinition definition)
        {
            if (definition == null)
            {
                Debug.LogError("CatSkinDefinition is null.", this);
                return;
            }

            ApplyPartVariant(_bodyResolver, definition.Body);
            ApplyPartVariant(_tailResolver, definition.Tail);
            ApplyPartVariant(_eyesResolver, definition.Eyes);
            ApplyPartVariant(_accessoryFrontResolver, definition.AccessoryFront);
            ApplyPartVariant(_accessoryBackResolver, definition.AccessoryBack);
        }

        private void ApplyPartVariant(SpriteResolver resolver, SpritePartVariant part)
        {
            if (resolver == null || part == null)
                return;

            resolver.SetCategoryAndLabel(part.Category, part.Label);
            resolver.ResolveSpriteToSpriteRenderer();
        }
    }
}