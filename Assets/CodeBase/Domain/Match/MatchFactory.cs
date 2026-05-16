using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match.Module;
using CodeBase.Domain.Match.Rules;
using CodeBase.Domain.Match.Rules.ScoreCalculator;

namespace CodeBase.Domain.Match
{
    public class MatchFactory
    {
        private readonly IMatchRules _rules;
        private readonly IFieldScoreCalculator _scoreCalculator;

        public MatchFactory(IMatchRules rules, IFieldScoreCalculator  scoreCalculator)
        {
            _rules = rules;
            _scoreCalculator = scoreCalculator;
        }

        public Match Create(IReadOnlyList<PlayerId> players, PlayerId firstPlayer) => 
            new(new MatchState(players, firstPlayer), _rules, _scoreCalculator);
    }
}