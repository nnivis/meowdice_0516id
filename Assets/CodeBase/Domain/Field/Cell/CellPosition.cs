namespace CodeBase.Domain.Field.Cell
{
    public readonly struct CellPosition
    {
        public int Col { get; }
        public int Row { get; }

        public CellPosition(int col, int row)
        {
            Col = col;
            Row = row;
        }
    }
}