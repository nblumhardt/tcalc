using System;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace tcalc.Parsing
{
    public static class ExpressionTokenizer
    {
        public static TextParser<TimeSpan> Magnitude { get; } =
            Character.EqualTo('d').Value(TimeSpan.FromDays(1))
                .Or(Character.EqualTo('h').Value(TimeSpan.FromHours(1)))
                .Or(Span.EqualTo("ms").Try().Value(TimeSpan.FromMilliseconds(1)))
                .Or(Character.EqualTo('m').Value(TimeSpan.FromMinutes(1)))
                .Or(Character.EqualTo('s').Value(TimeSpan.FromSeconds(1)));

        public static TextParser<TimeSpan> Duration { get; } =
            Numerics.DecimalDouble
                .Then(d => Magnitude.Select(m => m * d));

        static Tokenizer<ExpressionToken> Tokenizer { get; } = new TokenizerBuilder<ExpressionToken>()
            .Match(Character.EqualTo('+'), ExpressionToken.Plus)
            .Match(Character.EqualTo('-'), ExpressionToken.Minus)
            .Match(Character.EqualTo('*'), ExpressionToken.Asterisk)
            .Match(Character.EqualTo('/'), ExpressionToken.Slash)
            .Match(Duration, ExpressionToken.Duration, requireDelimiters: true)
            .Match(Numerics.Decimal, ExpressionToken.Number, requireDelimiters: true)
            .Match(Character.EqualTo('('), ExpressionToken.LParen)
            .Match(Character.EqualTo(')'), ExpressionToken.RParen)
            .Ignore(Span.WhiteSpace)
            .Match(Character.LetterOrDigit.AtLeastOnce(), ExpressionToken.Text, requireDelimiters: true)
            .Build();

        public static Result<TokenList<ExpressionToken>> TryTokenize(string source)
        {
            return Tokenizer.TryTokenize(source);
        }
    }
}
