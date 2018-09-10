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
                .Token()
                .Select(d => (Expression)new NumericValue(d));

        public static TextParser<TimeSpan> Magnitude { get; } =
            Character.EqualTo('d').Value(TimeSpan.FromDays(1))
                .Or(Character.EqualTo('h').Value(TimeSpan.FromHours(1)))
                .Or(Span.EqualTo("ms").Try().Value(TimeSpan.FromMilliseconds(1)))
                .Or(Character.EqualTo('m').Value(TimeSpan.FromMinutes(1)))
                .Or(Character.EqualTo('s').Value(TimeSpan.FromSeconds(1)));

        public static TextParser<Expression> Duration { get; } =
            Numerics.DecimalDouble
                .Then(d => Magnitude.Select(m => m * d))
                .Token()
                .Select(ts => (Expression)new DurationValue(ts));

        public static TextParser<Operator> Op(char symbol, Operator op) => 
            Character.EqualTo(symbol)
                .Token()
                .Value(op);

        public static TextParser<Operator> Add = Op('+', Operator.Add);
        public static TextParser<Operator> Subtract = Op('-', Operator.Subtract);
        public static TextParser<Operator> Multiply = Op('*', Operator.Multiply);
        public static TextParser<Operator> Divide = Op('/', Operator.Divide);

        public static TextParser<Expression> Literal = Duration.Try().Or(Number);

        static TextParser<Expression> Factor { get; } =
            (from lparen in Character.EqualTo('(').Token()
                from expr in Parse.Ref(() => Expression)
                from rparen in Character.EqualTo(')').Token()
                select expr)
            .Or(Literal);

        static readonly TextParser<Expression> Term = Parse.Chain(Multiply.Or(Divide), Factor, BinaryExpression.Create);
        static readonly TextParser<Expression> Expression = Parse.Chain(Add.Or(Subtract), Term, BinaryExpression.Create);

        static readonly TextParser<Expression> Source = Expression.AtEnd();

        public static bool TryParse(string input, out Expression expr, out string error)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = Source(new TextSpan(input));
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
