using System;
using System.Threading;
using Superpower;
using Superpower.Parsers;

// ReSharper disable UnusedMember.Global

namespace tcalc.Parsing
{
    static class TextParserExtensions
    {
        public static TextParser<T> Token<T>(this TextParser<T> parser)
        {
            return from _ in Span.WhiteSpace.Optional()
                from value in parser
                from __ in Span.WhiteSpace.Optional()
                select value;
        }

        static int _instance;

        public static TextParser<T> Log<T>(this TextParser<T> parser, string name)
        {
            var id = Interlocked.Increment(ref _instance);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{id}] Constructing instance of {name}");
            Console.ResetColor();
            return i =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{id}] Invoking with input: {i}");
                Console.ResetColor();
                var result = parser(i);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{id}] Result: {result}");
                Console.ResetColor();
                return result;
            };
        }
    }
}
