using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Abstracts;
using FParsec;
using FParsec.CSharp;

namespace Core
{
    public class LipsSimulator : StaticConstructor<LipsSimulator>
    {
        private readonly LispParser _parser;

        public LipsSimulator()
        {
            _parser = LispParser.New();
        }

        public Func<T> Simmulate<T>(string program)
        {
            var contour = new Dictionary<string, Expression>();

            var codeBlocks = program.Split(Environment.NewLine)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Where(x => !x.StartsWith("--"))
                .Select(x => x.Trim())
                .ToList();

            var expressions = codeBlocks
                .Select((codeBlock, i) =>
                {
                    var (status, result, _) = _parser.Parse(codeBlock);

                    if (status == ReplyStatus.Ok)
                    {
                        var body = new LipsToExpression().Resolve(result, contour);

                        return Expression.Lambda(codeBlocks.Count == i + 1 ? Expression.Convert(body, typeof(T)) : body);
                    }

                    throw new Exception("Parsing failed for: " + codeBlock);
                })
                .ToList();

            var d = expressions.Last().Compile();

            return () => (T)d.DynamicInvoke();
        }
    }
}