using System;
using UnityEngine;

namespace CodeBase.Domain.Dice
{
    [CreateAssetMenu(fileName = "DiceViewContent", menuName = "Dice/DiceViewContent", order = 2)]
    public class DiceViewContent : ScriptableObject
    {
        [Header("Indexed by DiceStateType (Green, Blue, Yellow)")] [SerializeField]
        private Sprite[] _stateSprites;

        [Header("Indexed by DicePointType (One..Six)")] [SerializeField]
        private Sprite[] _pointSprites;

        [SerializeField] private Sprite _fallback;

        public Sprite GetSprite<TEnum>(TEnum type) where TEnum : Enum =>
            ResolveSprite(type);

        private Sprite ResolveSprite<TEnum>(TEnum type) where TEnum : Enum
        {
            if (type is DiceStateType state) return GetStateSpriteInternal(state);
            if (type is DicePointType point) return GetPointSpriteInternal(point);

            throw new ArgumentException(
                $"Unsupported enum type: {typeof(TEnum).Name}. Expected DiceStateType or DicePointType.");
        }

        private Sprite GetStateSpriteInternal(DiceStateType type) =>
            GetByIndex<DiceStateType>(_stateSprites, (int)type);

        private Sprite GetPointSpriteInternal(DicePointType type) =>
            GetByIndex<DicePointType>(_pointSprites, (int)type);

        private Sprite GetByIndex<TEnum>(Sprite[] arr, int index)
            where TEnum : struct, Enum
        {
            if (arr == null || arr.Length == 0)
                return _fallback;

            if ((uint)index >= (uint)arr.Length)
                return _fallback;

            return arr[index] != null ? arr[index] : _fallback;
        }
    }
}