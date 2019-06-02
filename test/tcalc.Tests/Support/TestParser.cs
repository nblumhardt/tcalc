using System;
using Superpower;
using Superpower.Model;
using tcalc.Parsing;

// ReSharper disable UnusedMember.Global

namespace tcalc.Tests.Support
{
    static class TestParser
    {
        public static bool TryParseAll<T>(TextParser<T> parser, string source, out T value, out string error)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = parser.AtEnd()(new TextSpan(source));
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

        public static bool TryParseAll<T>(TokenListParser<ExpressionToken, T> parser, string source, out T value, out string error)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var tokens = ExpressionTokenizer.TryTokenize(source);
            if (!tokens.HasValue)
            {
                value = default;
                error = tokens.ToString();
                return false;
            }

            var result = parser.AtEnd()(tokens.Value);
            if (!result.HasValue)
            {
                value = default;
                error = result.ToString();
                return false;
            }

            value = result.Value;
            error = null;
            return true;
        }
    }
}
