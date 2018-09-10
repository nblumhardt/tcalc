using System;

namespace tcalc.Evaluator
{
    public class DurationResult : Result
    {
        public TimeSpan Value { get; }

        public DurationResult(TimeSpan value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString("c");
        }
    }
}