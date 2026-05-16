using System;

namespace CodeBase.Data.PlayerDataComponents
{
    public readonly struct PlayerId : IEquatable<PlayerId>
    {
        public PlayerId(int value) => Value = value;

        public int Value { get; }

        public bool Equals(PlayerId other) => Value == other.Value;

        public override bool Equals(object obj) => obj is PlayerId other && Equals(other);

        public override int GetHashCode() => Value;
    }
}