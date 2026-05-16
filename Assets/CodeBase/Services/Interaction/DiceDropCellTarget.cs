using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Field.View;
using UnityEngine;

namespace CodeBase.Services.Interaction
{
    public class DiceDropCellTarget : MonoBehaviour, IDiceDropCellTarget
    {
        private CellView _cellView;
        private FieldView _fieldView;

        public DiceDropTargetInfo TargetInfo =>
            new DiceDropTargetInfo(_fieldView.Slot, _cellView.Position);

        private void Awake()
        {
            _cellView = GetComponent<CellView>()
                        ?? GetComponentInParent<CellView>()
                        ?? GetComponentInChildren<CellView>();

            _fieldView = GetComponentInParent<FieldView>();

            if (_cellView == null)
                Debug.LogError($"CellView not found on {gameObject.name}");

            if (_fieldView == null)
                Debug.LogError($"FieldView not found on {gameObject.name}");
        }
    }
}