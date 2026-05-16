using System;
using Random = System.Random;

namespace CodeBase.Domain.Dice
{
    public class Dice
    {
        private static readonly Random Random = new();
        public DiceStateType DiceStateType { get; private set; }
        public DicePointType DicePointType { get; private set; }

        public int Value => DicePointType switch
        {
            DicePointType.One => 1,
            DicePointType.Two => 2,
            DicePointType.Three => 3,
            DicePointType.Four => 4,
            DicePointType.Five => 5,
            DicePointType.Six => 6,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        public void SetState(DiceStateType state) => DiceStateType = state;

        public Dice Roll()
        {
            return new Dice
            {
                DicePointType = GetRandomEnum<DicePointType>(),
                DiceStateType = DiceStateType.Normal
            };
        }

        private static T GetRandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Next(values.Length));
        }
    }

    public enum DiceStateType
    {
        Normal,
        MatchedPair,
        FullColumn
    }

    public enum DicePointType
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six
    }
}