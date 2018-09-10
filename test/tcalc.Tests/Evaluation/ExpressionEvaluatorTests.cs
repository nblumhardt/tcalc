using System;
using tcalc.Evaluation;
using tcalc.Expressions;
using Xunit;

namespace tcalc.Tests.Evaluation
{
    public class ExpressionEvaluatorTests
    {
        [Fact]
        public void AddingNumbersYieldsANumber()
        {
            var left = new NumericValue(5);
            var right = new NumericValue(3);
            var expr = new BinaryExpression(Operator.Add, left, right);
            var result = ExpressionEvaluator.Evaluate(expr);
            var actual = Assert.IsType<NumericResult>(result);
            Assert.Equal(8, actual.Value);
        }

        [Fact]
        public void EvaluationProceedsRecursively()
        {
            var mulLeft = new NumericValue(5);
            var mulRight = new NumericValue(3);
            var mulExpr = new BinaryExpression(Operator.Multiply, mulLeft, mulRight);

            var divLeft = new DurationValue(TimeSpan.FromMinutes(60));
            var divExpr = new BinaryExpression(Operator.Divide, divLeft, mulExpr);

            var result = ExpressionEvaluator.Evaluate(divExpr);
            var actual = Assert.IsType<DurationResult>(result);
            Assert.Equal(TimeSpan.FromMinutes(4), actual.Value);
        }

        [Fact]
        public void UncomputableExpressionsThrow()
        {
            var left = new NumericValue(5);
            var right = new DurationValue(TimeSpan.FromSeconds(3));
            var expr = new BinaryExpression(Operator.Divide, left, right);
            Assert.Throws<EvaluationException>(() => ExpressionEvaluator.Evaluate(expr));
        }
    }
}
