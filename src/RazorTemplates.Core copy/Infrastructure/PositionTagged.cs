using System;

namespace Rhythm.Text.Infrastructure
{
    /// <summary>
    /// Position tagged value for support RazorEngine 2.0
    /// </summary>
    public class PositionTagged<T>
    {
        /// <summary>
        /// Gets tag position.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets tag value.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of PositionTagged class.
        /// </summary>
        public PositionTagged(T value, int offset)
        {
            Position = offset;
            Value = value;
        }

        /// <summary>
        /// Converts tuple to PositionTagged object.
        /// </summary>
        public static implicit operator PositionTagged<T>(Tuple<T, int> value)
        {
            return new PositionTagged<T>(value.Item1, value.Item2);
        }
    }
}