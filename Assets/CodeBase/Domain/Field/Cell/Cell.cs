namespace CodeBase.Domain.Field.Cell
{
    public class Cell
    {
        public CellPosition Position { get; }

        public int Row => Position.Row;
        public int Column => Position.Col;

        public Domain.Dice.Dice Dice { get; private set; }
        public bool IsEmpty => Dice == null;

        public Cell(CellPosition position)
        {
            Position = position;
        }

        public bool TryPlaceDice(Domain.Dice.Dice dice)
        {
            if (!IsEmpty)
                return false;

            Dice = dice;
            return true;
        }

        public void Clear() => Dice = null;
    }
}