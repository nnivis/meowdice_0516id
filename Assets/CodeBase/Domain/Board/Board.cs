using System;
using UnityEngine;

namespace CodeBase.Domain.Board
{
    public class Board
    {
        public event Action<Dice.Dice> DiceChanged;

        public Dice.Dice Dice { get; private set; }

        public void RollDice()
        {

            var roller = new Dice.Dice();   // создаём временный объект
            Dice = roller.Roll();           // он генерирует новый случайный Dice

            DiceChanged?.Invoke(Dice);
        }

        public void ClearDice()
        {
            Dice = null;
            DiceChanged?.Invoke(null);
        }
        
    }
}