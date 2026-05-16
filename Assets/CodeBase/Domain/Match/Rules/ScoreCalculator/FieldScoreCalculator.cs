using System.Collections.Generic;

namespace CodeBase.Domain.Match.Rules.ScoreCalculator
{
    public class FieldScoreCalculator : IFieldScoreCalculator
    {
        public int ComputeColumnScore(IReadOnlyList<int> values)
        {
            var counts = new Dictionary<int, int>();

            for (int i = 0; i < values.Count; i++)
            {
                var value = values[i];

                counts.TryGetValue(value, out var count);
                counts[value] = count + 1;
            }

            var sum = 0;

            foreach (var pair in counts)
            {
                var value = pair.Key;
                var count = pair.Value;
                sum += value * count * count;
            }

            return sum;
        }

        public int ComputeTotalScore(Field.Field field)
        {
            var total = 0;

            for (int col = 0; col < field.ColumnsCount; col++)
                total += ComputeColumnScore(field.GetColumnDiceValues(col));

            return total;
        }
    }
}