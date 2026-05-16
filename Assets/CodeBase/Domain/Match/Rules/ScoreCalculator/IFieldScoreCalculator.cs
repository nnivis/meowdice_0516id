using System.Collections.Generic;

namespace CodeBase.Domain.Match.Rules.ScoreCalculator
{
    public interface IFieldScoreCalculator
    {
        int ComputeColumnScore(IReadOnlyList<int> values);
        int ComputeTotalScore(Field.Field field);
    }
}