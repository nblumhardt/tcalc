using System;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using tcalc.Expressions;

namespace tcalc.Parsing
{
    public static class ExpressionParser
    {
        public static TextParser<Expression> Number { get; } =
            Numerics.DecimalDouble
                .Select(d => (Expression)new NumericValue(d))
                .Token();

        public static TextParser<TimeSpan> Magnitude { get; } =
            Character.EqualTo('d').Value(TimeSpan.FromDays(1))
                .Or(Character.EqualTo('h').Value(TimeSpan.FromHours(1)))
                .Or(Span.EqualTo("ms").Try().Value(TimeSpan.FromMilliseconds(1)))
                .Or(Character.EqualTo('m').Value(TimeSpan.FromMinutes(1)))
                .Or(Character.EqualTo('s').Value(TimeSpan.FromSeconds(1)));

        public static TextParser<Expression> Duration { get; } =
            Numerics.DecimalDouble
                .Then(d => Magnitude.Select(m => m * d))
                .Select(ts => (Expression)new DurationValue(ts))
                .Token();

        public static TextParser<Expression> Expression = Duration.Or(Number).AtEnd();

        public static bool TryParse(string input, out Expression expr, out string error)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = Expression(new TextSpan(input));
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
