using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Abstracts;
using ExpressionTreeToString;
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
            var contour = new Contour<Expression>();

            var codeBlocks = program.Split(Environment.NewLine)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Where(x => !x.StartsWith("--"))
                .Select(x => x.Trim())
                .ToList();

            var expressions = codeBlocks
                .Select((codeBlock, i) =>
                {
                    var (status, result, error) = _parser.Parse(codeBlock);

                    if (status == ReplyStatus.Ok)
                    {
                        var body = new LipsToExpression().Resolve(result, contour);

                        return Expression.Lambda(body);
                    }

                    throw new Exception("Parsing failed for: " + codeBlock + error);
                })
                .ToList();
            
#if DEBUG
            var compiledCode = string.Join("\n\n", expressions.Select(x => x.ToString("C#")));
#endif
            
            Console.WriteLine(compiledCode);
            
            var d = expressions.Last().Compile();

            return () => (T)Convert.ChangeType(d.DynamicInvoke(), typeof(T));
        }
    }
}