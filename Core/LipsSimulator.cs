using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Abstracts;
using ExpressionTreeToString;
using FParsec;
using FParsec.CSharp;

namespace Core
{
    public class LipsSimulator
    {
        private readonly LispParser _parser;
        
        private readonly LipsToExpression _expressionWriter;

        public LipsSimulator()
        {
            _parser = new LispParser();
            _expressionWriter = new LipsToExpression();
        }

        public Func<T> Simmulate<T>(string program)
        {
            var contour = new Contour<Expression>();

           var (status, result, error) = _parser.ExpressionsP.ParseString(program);
           
           if (status == ReplyStatus.Ok)
           {
               var expressions = result
                   .Select((codeBlock, i) =>
                   {
                       var body = _expressionWriter.Resolve(codeBlock, contour);

                       return Expression.Lambda(body);

                   })
                   .ToList();

               var d = expressions.Last().Compile();

               return () => (T)Convert.ChangeType(d.DynamicInvoke(), typeof(T));
           }

           throw new Exception("Parsing failed for: " + error.Head);
        }
    }
}