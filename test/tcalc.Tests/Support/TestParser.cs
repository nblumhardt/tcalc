using System;
using Superpower;
using Superpower.Model;

namespace tcalc.Tests.Support
{
    static class TestParser
    {
        public static bool TryParseAll<T>(TextParser<T> parser, string input, out T value, out string error)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = parser.AtEnd()(new TextSpan(input));
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
