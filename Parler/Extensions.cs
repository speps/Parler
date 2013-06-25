using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Parler
{
    public static class ParserExtensions
    {
        public static string Name<T>(this Expression<Func<Parser<T>>> parser)
        {
            return (parser.Body as MemberExpression).Member.Name;
        }

        //public static Parser<T> Or<T>(this Parser<T> first, Parser<T> second)
        //{
        //    return new DisjunctiveParser<T>(first, second);
        //}

        //public static Parser<IEnumerable<T>> Many<T>(this Parser<T> parser)
        //{
        //    return new ManyParser<T>(parser);
        //}

        //public static Parser<U> Select<T, U>(this Parser<T> parser, Func<T, U> selector)
        //{
        //    return parser.Then(t => Parse.Return(selector(t)));
        //}

        //public static Parser<U> Return<T, U>(this Parser<T> parser, U value)
        //{
        //    return parser.Select(t => value);
        //}

        //public static Parser<IEnumerable<T>> Once<T>(this Parser<T> parser)
        //{
        //    return parser.Select(t => (IEnumerable<T>) new T[] { t });
        //}

        //public static Parser<U> Then<T, U>(this Parser<T> first, Func<T, Parser<U>> second)
        //{
        //    return new SequentialParser<T, U>(first, second);
        //}

        //public static Parser<U> Then<T, U>(this Parser<T> first, Parser<U> second)
        //{
        //    return new SequentialParser<T, U>(first, v => second);
        //}

        //public static Parser<string> Text(this Parser<IEnumerable<char>> parser)
        //{
        //    return parser.Select(ch => string.Concat(ch));
        //}
    }
}
