using System;
using tcalc.Evaluator;
using tcalc.Expressions;

namespace tcalc
{
    class Program
    {
        static void Main()
        {
            Console.Write("tcalc> ");
            var line = Console.ReadLine();
            while (line != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        // TODO: Parse the input `line` :-)
                        var left = new NumericValue(5);
                        var right = new NumericValue(3);
                        var expr = new BinaryExpression(left, right, Operator.Add);

                        var result = ExpressionEvaluator.Evaluate(expr);

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(result);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                    }

                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.Write("tcalc> ");
                line = Console.ReadLine();
            }
        }
    }
}
