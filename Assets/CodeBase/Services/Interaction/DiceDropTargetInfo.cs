using CodeBase.Domain;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Services.Interaction
{
    public readonly struct DiceDropTargetInfo
    {
        public DiceDropTargetInfo(PlayerSlot slot, CellPosition cellPosition)
        {
            Slot = slot;
            CellPosition = cellPosition;
        }

        public PlayerSlot Slot { get; }
        public CellPosition CellPosition { get; }
    }
}