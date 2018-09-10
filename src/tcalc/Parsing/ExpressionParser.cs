using System;
using tcalc.Expressions;

namespace tcalc.Parsing
{
    public static class ExpressionParser
    {
        public static Parser<Expression> Number { get; } =
            Parse.Double
                .Select(d => (Expression)new NumericValue(d))
                .Token();

        public static Parser<TimeSpan> Magnitude { get; } =
            Parse.Char('d').Value(TimeSpan.FromDays(1))
                .Or(Parse.Char('h').Value(TimeSpan.FromHours(1)))
                .Or(Parse.Char('m').Then(_ => Parse.Char('s').Value(TimeSpan.FromMilliseconds(1))))
                .Or(Parse.Char('m').Value(TimeSpan.FromMinutes(1)))
                .Or(Parse.Char('s').Value(TimeSpan.FromSeconds(1)));

        public static Parser<Expression> Duration { get; } =
            Parse.Double
                .Then(d => Magnitude.Select(m => m * d))
                .Select(ts => (Expression)new DurationValue(ts))
                .Token();

        public static Parser<Expression> Expression = Duration.Or(Number).AtEnd();

        public static bool TryParse(string input, out Expression expr, out string error)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = Expression(new Input(input));
            if (result.HasValue)
            {
                expr = result.Value;
                error = null;
                return true;
            }

            expr = null;
            error = result.ToString();
            return false;
        }
    }
}
