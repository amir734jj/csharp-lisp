using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Abstracts;
using Core.Interfaces;
using Core.Tokens;

namespace Core
{
    public class LipsToExpression : Visitor<Expression>
    {
        private IDictionary<string, Expression> _contour = new Dictionary<string, Expression>();
        
        public Expression Resolve(IToken token, IDictionary<string, Expression> contour)
        {
            _contour = contour;
            
            return Visit(token);
        }
        
        public override Expression Visit(UnaryToken unaryToken)
        {
            var (expressionKey, type) = MapUnaryOpToExpressionType(unaryToken.Op);

            return Expression.MakeUnary(expressionKey, Expression.Convert(Visit(unaryToken.Expr), type), type);
        }
        
        public override Expression Visit(BinaryOperatorToken binaryOperatorToken)
        {
            var (expressionKey, type) = MapBinaryOpToExpressionType(binaryOperatorToken.Op);

            return Expression.MakeBinary(expressionKey, Expression.Convert(Visit(binaryOperatorToken.Expr1), type), Expression.Convert(Visit(binaryOperatorToken.Expr2), type));
        }

        public override Expression Visit(NumberToken numberToken)
        {
            return Expression.Constant(numberToken.Number);
        }

        public override Expression Visit(ConditionalToken conditionalToken)
        {
            return Expression.Condition(Visit(conditionalToken.CondExpr), Visit(conditionalToken.IfExpr), Visit(conditionalToken.ElseExpr));
        }
        
        public override Expression Visit(ParameterToken parameterToken)
        {
            return _contour[parameterToken.Name];
        }

        public override Expression Visit(FunctionCallToken functionCallToken)
        {
            var flag = _contour.TryGetValue(functionCallToken.Name, out var functionDef);

            if (!flag)
            {
                functionDef = Expression.Convert(Expression.Property(Expression.Constant(_contour), "Item", Expression.Constant(functionCallToken.Name)), typeof(LambdaExpression));
            }

     
            return Expression.Invoke(functionDef, functionCallToken.Token.Select(Visit).Select(x => Expression.Convert(x, typeof(object))).ToList());
        }

        public override Expression Visit(FunctionDefToken functionDefToken)
        {
            var formals = functionDefToken.Formals.Select(x => (Key: x, Formal: Expression.Parameter(typeof(object), x))).ToList();
            
            foreach (var (key, expression) in formals)
            {
                _contour[key] = expression;
            }
            
            var functionExpr = Expression.Lambda(Visit(functionDefToken.Body), formals.Select(x => x.Formal));
            
            _contour[functionDefToken.Name] = functionExpr;

            return functionExpr;
        }

        private static (ExpressionType expressionKey, Type type) MapBinaryOpToExpressionType(string op)
        {
            return op switch
            {
                "<" => (ExpressionType.LessThan, typeof(double)),
                "<=" => (ExpressionType.LessThanOrEqual, typeof(double)),
                ">" => (ExpressionType.GreaterThan, typeof(double)),
                "+" => (ExpressionType.Add, typeof(double)),
                "-" => (ExpressionType.Subtract, typeof(double)),
                "/" => (ExpressionType.Divide, typeof(double)),
                "*" => (ExpressionType.Multiply, typeof(double)),
                "==" => (ExpressionType.Equal, typeof(object)),
                "!=" => (ExpressionType.NotEqual, typeof(object))
            };
        }
        
        private static (ExpressionType expressionKey, Type type) MapUnaryOpToExpressionType(string op)
        {
            return op switch
            {
                "!" => (ExpressionType.Not, typeof(bool)),
                "-" => (ExpressionType.Negate, typeof(double)),
            };
        }
    }
}