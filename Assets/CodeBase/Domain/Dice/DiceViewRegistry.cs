using System.Collections.Generic;

namespace CodeBase.Domain.Dice
{
    public class DiceViewRegistry : IDiceViewRegistry
    {
        private readonly Dictionary<Dice, DiceView> _viewByDice = new();
        private readonly Dictionary<DiceView, Dice> _diceByView = new();

        public void Register(Dice dice, DiceView view)
        {
            if (dice == null || view == null)
                return;

            _viewByDice[dice] = view;
            _diceByView[view] = dice;
        }

        public void Unregister(Dice dice)
        {
            if (dice == null)
                return;

            if (_viewByDice.TryGetValue(dice, out var view))
                _diceByView.Remove(view);

            _viewByDice.Remove(dice);
        }

        public bool TryGetView(Dice dice, out DiceView view) =>
            _viewByDice.TryGetValue(dice, out view);

        public bool TryGetDice(DiceView view, out Dice dice) =>
            _diceByView.TryGetValue(view, out dice);
    }
}