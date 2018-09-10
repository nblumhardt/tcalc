using System;

namespace tcalc.Parsing
{
    public struct Input : IEquatable<Input>
    {
        readonly string _source;
        readonly int _position;

        public Input(string source)
            : this(source, 0)
        {
        }

        Input(string source, int position)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _position = position;
        }

        public static Input Empty = default(Input);

        public bool IsAtEnd
        {
            get
            {
                EnsureNotEmpty();
                return _source.Length == _position;
            }
        }

        void EnsureNotEmpty()
        {
            if (_source == null)
                throw new InvalidOperationException("Input is empty.");
        }

        public Result<char> NextChar()
        {
            EnsureNotEmpty();

            if (IsAtEnd)
                return Result.Empty<char>(this);

            return Result.Value(_source[_position], new Input(_source, _position + 1));
        }

        public override bool Equals(object obj)
        {
            return obj is Input input && Equals(input);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_source?.GetHashCode() ?? 0) * 397) ^ _position;
            }
        }

        public bool Equals(Input other)
        {
            return string.Equals(_source, other._source) && _position == other._position;
        }

        public static bool operator ==(Input lhs, Input rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Input lhs, Input rhs)
        {
            return !(lhs == rhs);
        }
    }
}