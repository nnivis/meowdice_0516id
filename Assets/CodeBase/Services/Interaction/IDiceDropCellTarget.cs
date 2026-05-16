using CodeBase.Domain.Field.Cell;

namespace CodeBase.Services.Interaction
{
    public interface IDiceDropCellTarget
    {
        DiceDropTargetInfo TargetInfo { get; }
    }
}