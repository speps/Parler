using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Parler;

namespace ScratchExample
{
    public static class Ext
    {
        public static string Name(this Expression<Func<Caca>> parser)
        {
            return (parser.Body as MemberExpression).Member.Name;
        }

        public static U Select<U>(this Expression<Func<Caca>> parser, Func<Caca, U> convert)
        {
            return convert(new Caca { Test = parser.Name() });
        }
    }

    public class Caca
    {
        public string Test;
    }

    class Program
    {
        static Expression<Func<Caca>> W(Expression<Func<Caca>> f)
        {
            return f;
        }

        //public static readonly Parser<string> S = (Parse.Ref(() => S).Then(Parse.Chars("a").Once()).Or(Parse.Chars("a").Once())).Text();

        static void Main(string[] args)
        {
            var c = new Caca { Test = "hello" };

            var q = from t in W(() => c)
                    select t;

            //var p = Parse.Chars("+-").Many().Text().Select(s => new Caca { Test = s }).Or(Parse.Chars("*/").Many().Text().Select(s => new Caca { Test = s })).Many();
            //var result = Parse.TryParse(p, "+-**/");
            //var result = S.TryParse("aaa");
        }
    }
}
