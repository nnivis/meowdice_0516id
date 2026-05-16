using UnityEngine;

namespace CodeBase.Domain.Character.CatSkin.Content
{
    [CreateAssetMenu(
        fileName = "CatSkinDefinition",
        menuName = "Character/Cat Skin Definition")]
    public class CatSkinDefinition : ScriptableObject
    {
        [SerializeField] private CatSkinId _skinId;

        [Header("Parts")]
        [SerializeField] private SpritePartVariant _body;
        [SerializeField] private SpritePartVariant _tail;
        [SerializeField] private SpritePartVariant _eyes;
        [SerializeField] private SpritePartVariant _accessoryFront;
        [SerializeField] private SpritePartVariant _accessoryBack;

        public CatSkinId SkinId => _skinId;
        public SpritePartVariant Body => _body;
        public SpritePartVariant Tail => _tail;
        public SpritePartVariant Eyes => _eyes;
        public SpritePartVariant AccessoryFront => _accessoryFront;
        public SpritePartVariant AccessoryBack => _accessoryBack;
    }
}