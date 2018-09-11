using System;
using tcalc.Expressions;
using tcalc.Parsing;
using tcalc.Tests.Support;
using Xunit;

namespace tcalc.Tests.Parsing
{
    public class ExpressionParserTests
    {
        [Theory]
        [InlineData("0", 0.0)]
        [InlineData("0.0", 0.0)]
        [InlineData("1", 1.0)]
        [InlineData("1.0", 1.0)]
        [InlineData("0.120", 0.12)]
        [InlineData("0.004", 0.004)]
        [InlineData("123", 123.0)]
        [InlineData("0123.45", 123.45)]
        [InlineData(" 123 ", 123.0)]
        public void ValidNumbersAreParsed(string input, double expected)
        {
            Assert.True(TestParser.TryParseAll(ExpressionParser.Number, input, out var expr, out var err), err);
            var numeric = Assert.IsType<NumericValue>(expr);
            Assert.Equal(expected, numeric.Value);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("1. 2")]
        [InlineData("1.a")]
        [InlineData("a0")]
        [InlineData("!!!")]
        public void InvalidNumbersAreRejected(string input)
        {
            Assert.False(TestParser.TryParseAll(ExpressionParser.Number, input, out _, out _));
        }

        [Fact]
        public void WholeDurationsAreParsed()
        {
            Assert.True(TestParser.TryParseAll(ExpressionParser.Duration, "150h", out var expr, out var err), err);
            var duration = Assert.IsType<DurationValue>(expr);
            Assert.Equal(TimeSpan.FromHours(150), duration.Value);
        }

        [Fact]
        public void FractionalDurationsAreParsed()
        {
            Assert.True(TestParser.TryParseAll(ExpressionParser.Duration, "1.5ms", out var expr, out var err), err);
            var duration = Assert.IsType<DurationValue>(expr);
            Assert.Equal(TimeSpan.FromTicks((long)(1.5 * TimeSpan.TicksPerMillisecond)), duration.Value);
        }

        [Fact]
        public void ZeroDurationsAreParsed()
        {
            Assert.True(TestParser.TryParseAll(ExpressionParser.Duration, "0m", out var expr, out var err), err);
            var duration = Assert.IsType<DurationValue>(expr);
            Assert.Equal(TimeSpan.Zero, duration.Value);
        }
    }
}
