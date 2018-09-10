using System;
using tcalc.Parsing;

namespace tcalc.Tests.Support
{
    static class TestParser
    {
        public static bool TryParseAll<T>(Parser<T> parser, string input, out T value, out string error)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = parser.AtEnd()(new Input(input));
            if (result.HasValue)
            {
                value = result.Value;
                error = null;
                return true;
            }

            value = default;
            error = result.ToString();
            return false;
        }
    }
}
