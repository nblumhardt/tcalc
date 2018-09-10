using System;
using System.Collections.Generic;
using System.Text;
using Superpower;
using Superpower.Parsers;

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
    }
}
