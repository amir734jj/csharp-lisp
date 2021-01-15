using System;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeToString;
using FParsec;
using FParsec.CSharp;
using ZSpitz.Util;

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
            var contour = _expressionWriter.EmptyContour();

           var (status, result, error) = _parser.ExpressionsP.ParseString(program);
           
           if (status == ReplyStatus.Ok)
           {
               var expressions = result
                   .Select((codeBlock, i) =>
                   {
                       var body = _expressionWriter.Resolve(codeBlock, contour);

                       return Expression.Lambda(body);
                   })
                   .Select(x =>
                   {
                       try
                       {
                           return x.Compile();
                       }
                       catch (Exception e)
                       {
                           var str = x.ToString("Object notation", "C#");
                           
                           throw new Exception(str, e);
                       }
                   })
                   .ToList();

               var sideEffects = expressions.SkipLast(1);
              
               return () =>
               {
                   foreach (var @delegate in sideEffects)
                   {
                       @delegate.DynamicInvoke();
                   }
                   
                   return (T) Convert.ChangeType(@expressions.Last().DynamicInvoke(), typeof(T));
               };
           }

           throw new Exception("Parsing failed for: " + error.Head);
        }
    }
}