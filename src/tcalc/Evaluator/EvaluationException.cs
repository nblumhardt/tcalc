using System;

namespace tcalc.Evaluator
{
    public class EvaluationException : Exception
    {
        public EvaluationException(string message)
            : base(message)
        {
        }
    }
}
