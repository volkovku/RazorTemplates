using System;

namespace Rhythm.Text.Templating.Infrastructure
{
    /// <summary>
    /// Html attribute value for support RazorEngine 2.0
    /// </summary>
    public class AttributeValue
    {
        /// <summary>
        /// Gets a prefix of attribute value.
        /// </summary>
        public PositionTagged<string> Prefix { get; private set; }

        /// <summary>
        /// Gets an attribute value.
        /// </summary>
        public PositionTagged<object> Value { get; private set; }

        /// <summary>
        /// Gets a flag which determines is attribute value is literal.
        /// </summary>
        public bool Literal { get; private set; }

        /// <summary>
        /// Initializes a new instance of AttributeValue class.
        /// </summary>
        public AttributeValue(PositionTagged<string> prefix, PositionTagged<object> value, bool literal)
        {
            Prefix = prefix;
            Value = value;
            Literal = literal;
        }

        /// <summary>
        /// Converts tuples to AttributeValue object.
        /// </summary>
        public static implicit operator AttributeValue(Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
        {
            return new AttributeValue(value.Item1, value.Item2, value.Item3);
        }

        /// <summary>
        /// Converts tuples to AttributeValue object.
        /// </summary>
        public static implicit operator AttributeValue(Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
        {
            return new AttributeValue(value.Item1, new PositionTagged<object>(value.Item2.Item1, value.Item2.Item2), value.Item3);
        }
    }
}