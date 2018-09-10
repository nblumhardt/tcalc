using System;

namespace tcalc.Parsing
{
    public static class Result
    {
        public static Result<T> Empty<T>(Input remainder)
        {
            return new Result<T>(remainder);
        }

        public static Result<T> Value<T>(T value, Input remainder)
        {
            return new Result<T>(value, remainder);
        }
    }

    public struct Result<T>
    {
        readonly T _value;

        public Input Remainder { get; }

        public bool HasValue { get; }

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("Result has no value.");
                return _value;
            }
        }

        public Result(T value, Input remainder)
            : this(remainder)
        {
            HasValue = true;
            _value = value;
        }

        public Result(Input remainder)
        {
            Remainder = remainder;
            _value = default(T);
            HasValue = false;
        }

        public override string ToString()
        {
            if (Remainder == Input.Empty)
                return "Empty result";

            if (HasValue)
                return "Value: " + _value;

            return Remainder.IsAtEnd ? 
                "Unexpected end of input" : 
                $"Unexpected '{Remainder.NextChar().Value}'";
        }
    }
}
