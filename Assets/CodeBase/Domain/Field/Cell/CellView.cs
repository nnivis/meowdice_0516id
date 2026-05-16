using CodeBase.Domain.Dice;
using UnityEngine;

namespace CodeBase.Domain.Field.Cell
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Transform _diceAnchor;

        private DiceView _diceView;

        public CellPosition Position { get; private set; }
        public bool IsEmpty => _diceView == null;
        public DiceView DiceView => _diceView;

        public void Initialize(CellPosition position) => Position = position;

        public void SetDiceView(DiceView diceView)
        {
            if (diceView == null)
                return;

            _diceView = diceView;

            Transform target = _diceAnchor != null ? _diceAnchor : transform;
            Transform diceTransform = diceView.transform;

            diceTransform.SetParent(target, false);
            diceTransform.localPosition = Vector3.zero;
            diceTransform.localRotation = Quaternion.identity;
            diceTransform.localScale = Vector3.one;
        }

        public void ClearDiceView()
        {
            if (_diceView != null)
                Destroy(_diceView.gameObject);

            _diceView = null;
        }

        public void Clear() => _diceView = null;
    }
}