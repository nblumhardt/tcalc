using System;
using tcalc.Evaluator;
using tcalc.Expressions;
using Xunit;

namespace tcalc.Tests.Evaluator
{
    public class ExpressionEvaluatorTests
    {
        [Fact]
        public void AddingNumbersYieldsANumber()
        {
            var left = new NumericValue(5);
            var right = new NumericValue(3);
            var expr = new BinaryExpression(left, right, Operator.Add);
            var result = ExpressionEvaluator.Evaluate(expr);
            var actual = Assert.IsType<NumericResult>(result);
            Assert.Equal(8, actual.Value);
        }

        [Fact]
        public void EvaluationProceedsRecursively()
        {
            var mulLeft = new NumericValue(5);
            var mulRight = new NumericValue(3);
            var mulExpr = new BinaryExpression(mulLeft, mulRight, Operator.Multiply);

            var divLeft = new DurationValue(TimeSpan.FromMinutes(60));
            var divExpr = new BinaryExpression(divLeft, mulExpr, Operator.Divide);

            var result = ExpressionEvaluator.Evaluate(divExpr);
            var actual = Assert.IsType<DurationResult>(result);
            Assert.Equal(TimeSpan.FromMinutes(4), actual.Value);
        }

        [Fact]
        public void UncomputableExpressionsThrow()
        {
            var left = new NumericValue(5);
            var right = new DurationValue(TimeSpan.FromSeconds(3));
            var expr = new BinaryExpression(left, right, Operator.Divide);
            Assert.Throws<EvaluationException>(() => ExpressionEvaluator.Evaluate(expr));
        }
    }
}
