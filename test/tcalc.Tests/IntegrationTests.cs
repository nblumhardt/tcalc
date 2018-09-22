using Superpower.Model;
using tcalc.Evaluation;
using tcalc.Parsing;
using Xunit;

namespace tcalc.Tests
{
    public class IntegrationTests
    {
        [Theory]
        [InlineData("1 + 2", "3")]
        [InlineData("1 + 2 * 3", "7")]
        [InlineData("(1 + 2) * 3", "9")]
        [InlineData("3d / 8h", "9")]
        [InlineData("1d + 2ms", "1.00:00:00.0020000")]
        [InlineData("(1h-10m)/40s", "75")]
        public void ValidResultsAreComputed(string source, string result)
        {
            var tokens = ExpressionTokenizer.TryTokenize(source);
            Assert.True(tokens.HasValue, tokens.ToString());

            Assert.True(ExpressionParser.TryParse(tokens.Value, out var expr, out var err, out var errorPosition), err);
            var actual = ExpressionEvaluator.Evaluate(expr);
            Assert.Equal(result, actual.ToString());
            Assert.Equal(errorPosition, new Position(0, 0, 0));
        }
    }
}
