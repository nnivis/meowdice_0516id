using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Services.Interaction
{
    public class DiceDragContext
    {
        public DiceView DiceView;
        public CellPosition? SourcePosition;
        public Transform OriginalParent;
        public int OriginalSiblingIndex;
        public CanvasGroup CanvasGroup;
        public Vector2 OriginalAnchoredPosition;
    }
}