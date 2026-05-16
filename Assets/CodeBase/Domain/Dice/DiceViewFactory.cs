using UnityEngine;

namespace CodeBase.Domain.Dice
{
    [CreateAssetMenu(fileName = "DiceFactory", menuName = "Dice/DiceFactory", order = 1)]
    public class DiceViewFactory : ScriptableObject
    {
        [SerializeField] private DiceView _diceViewPrefab;

        public DiceView CreateDice(Transform parent) => Instantiate(_diceViewPrefab, parent);
    }
}