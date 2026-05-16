namespace CodeBase.Domain.Dice
{
    public interface IDiceViewRegistry
    {
        void Register(Dice dice, DiceView view);
        void Unregister(Dice dice);
        bool TryGetView(Dice dice, out DiceView view);
        bool TryGetDice(DiceView view, out Dice dice);
    }
}