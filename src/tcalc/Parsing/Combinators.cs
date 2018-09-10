using System;
using System.Collections.Generic;

namespace tcalc.Parsing
{
    public static class Combinators
    {
        public static Parser<TValue> Value<TIgnored, TValue>(this Parser<TIgnored> parser, TValue value)
        {
            return parser.Then(_ => Parse.Constant(value));
        }

        public static Parser<TSecond> Then<TFirst, TSecond>(
            this Parser<TFirst> first,
            Func<TFirst, Parser<TSecond>> second)
        {
            return input =>
            {
                var rt = first(input);
                if (!rt.HasValue)
                    return Result.Empty<TSecond>(rt.Remainder);

                return second(rt.Value)(rt.Remainder);
            };
        }

        public static Parser<TTo> Select<TFrom, TTo>(this Parser<TFrom> source, Func<TFrom, TTo> selector)
        {
            return source.Then(rt => Parse.Constant(selector(rt)));
        }

        public static Parser<TItem[]> Many<TItem>(this Parser<TItem> item)
        {
            return input =>
            {
                var result = new List<TItem>();
                var r = item(input);
                while (r.HasValue)
                {
                    result.Add(r.Value);
                    r = item(r.Remainder);
                }
                return Result.Value(result.ToArray(), r.Remainder);
            };
        }

        public static Parser<T> Or<T>(this Parser<T> lhs, Parser<T> rhs)
        {
            return input =>
            {
                var first = lhs(input);
                return first.HasValue ? first : rhs(input);
            };
        }

        public static Parser<T> Token<T>(this Parser<T> content)
        {
            return Parse.WhiteSpace().Many()
                .Then(_ => content)
                .Then(c => Parse.WhiteSpace().Many().Select(_ => c));
        }

        public static Parser<T> AtEnd<T>(this Parser<T> parser)
        {
            return input =>
            {
                var result = parser(input);
                if (!result.HasValue || result.Remainder.IsAtEnd)
                    return result;

                return Result.Empty<T>(result.Remainder);
            };
        }
    }
}
