using System;
using System.Globalization;

namespace tcalc.Parsing
{
    public static class Parse
    {
        public static Parser<TValue> Constant<TValue>(TValue value)
        {
            return input => Result.Value(value, input);
        }

        public static Parser<char> CharMatching(Func<char, bool> predicate)
        {
            return input =>
            {
                var next = input.NextChar();
                if (!next.HasValue || !predicate(next.Value))
                    return Result.Empty<char>(input);

                return next;
            };
        }

        public static Parser<char> Char(char ch) => CharMatching(i => ch == i);

        public static Parser<char> WhiteSpace() => CharMatching(char.IsWhiteSpace);

        public static Parser<int> Natural { get; } = input =>
        {
            var next = input.NextChar();
            if (!next.HasValue || !char.IsDigit(next.Value))
                return Result.Empty<int>(input);

            Input remainder;
            var val = 0;
            do
            {
                val = 10 * val + CharUnicodeInfo.GetDigitValue(next.Value);
                remainder = next.Remainder;
                next = next.Remainder.NextChar();
            } while (next.HasValue && char.IsDigit(next.Value));

            return Result.Value(val, remainder);
        };

        static Parser<double> DecimalFraction { get; } = input =>
        {
            var fdigit = input.NextChar();
            if (!fdigit.HasValue || !char.IsDigit(fdigit.Value))
                return Result.Empty<double>(input);

            Input remainder;
            decimal result = 0;
            var place = 10.0m;
            do
            {
                result = result + CharUnicodeInfo.GetDigitValue(fdigit.Value) / place;
                remainder = fdigit.Remainder;
                fdigit = fdigit.Remainder.NextChar();
                place = place * 10;
            } while (fdigit.HasValue && char.IsDigit(fdigit.Value));

            return Result.Value((double)result, remainder);
        };

        public static Parser<double> Double { get; } =
            Natural.Then(n =>
                Char('.')
                    .Then(_ => DecimalFraction)
                    .Or(Constant(0.0))
                    .Select(f => n + f));
    }
}
