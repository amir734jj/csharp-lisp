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
        private Contour<Expression> _contour = new Contour<Expression>();


        public Contour<Expression> EmptyContour()
        {
            static object Print(object o)
            {
                Console.Write(o);
                return o;
            }

            var printP = Expression.Parameter(typeof(object));
            var printExpr = Expression.Lambda(Expression.Invoke(Expression.Constant((Func<object, object>) Print), printP), printP);

            static object Println(object o)
            {
                Console.WriteLine(o);
                return o;
            }

            var printlnP = Expression.Parameter(typeof(object));
            var printlnExpr = Expression.Lambda(Expression.Invoke(Expression.Constant((Func<object, object>) Println), printlnP), printlnP);

            static object Concat(object o1, object o2) => o1 + o2.ToString();
            var concatP1 = Expression.Parameter(typeof(object));
            var concatP2 = Expression.Parameter(typeof(object));
            var concatExpr = Expression.Lambda(Expression.Invoke(Expression.Constant((Func<object, object, object>) Concat), concatP1, concatP2), concatP1, concatP2);
            
            return new Contour<Expression>(new Dictionary<string, Expression>
            {
                ["print"] = printExpr,
                ["println"] = printlnExpr,
                ["concat"] = concatExpr
            });
        }
        
        public Expression Resolve(IToken token, Contour<Expression> contour)
        {
            _contour = contour;

            return Visit(token);
        }

        public override Expression Visit(StringToken stringToken)
        {
            return Expression.Constant(stringToken.Str);
        }
        
        public override Expression Visit(UnaryToken unaryToken)
        {
            var (expressionKey, type) = MapUnaryOpToExpressionType(unaryToken.Op);

            return Expression.MakeUnary(expressionKey, Expression.Convert(Visit(unaryToken.Expr), type), type);
        }

        public override Expression Visit(BinaryOperatorToken binaryOperatorToken)
        {
            var (expressionKey, type) = MapBinaryOpToExpressionType(binaryOperatorToken.Op);

            return Expression.MakeBinary(expressionKey, Expression.Convert(Visit(binaryOperatorToken.Expr1), type),
                Expression.Convert(Visit(binaryOperatorToken.Expr2), type));
        }

        public override Expression Visit(NumberToken numberToken)
        {
            return Expression.Constant(numberToken.Number);
        }

        public override Expression Visit(ConditionalToken conditionalToken)
        {
            return Expression.Condition(
                Visit(conditionalToken.CondExpr),
                Visit(conditionalToken.IfExpr),
                Visit(conditionalToken.ElseExpr)
            );
        }

        public override Expression Visit(ParameterToken parameterToken)
        {
            var flag = _contour.Lookup(parameterToken.Name, out var result);

            if (!flag)
            {
                throw new Exception($"Unbound parameter {parameterToken.Name}");
            }
            
            return result;
        }

        public override Expression Visit(Comment comment)
        {
            return Expression.Empty();
        }

        public override Expression Visit(FunctionCallToken functionCallToken)
        {
            var flag = _contour.Lookup(functionCallToken.Name, out var functionDef);

            if (!flag)
            {
                functionDef = Expression.Property(Expression.Constant(_contour), "Item", Expression.Constant(functionCallToken.Name));

                var genericFuncType =
                    typeof(Expression<>).MakeGenericType(StaticFuncType(functionCallToken.Token.Length));
                
                functionDef = Expression.Convert(functionDef, genericFuncType);

                functionDef = Expression.Invoke(Expression.Constant(Expression.Lambda(functionDef)));
                
                return Expression.Invoke(functionDef,
                    functionCallToken.Token.Select(Visit).Select(x => Expression.Convert(x, typeof(object))).ToList());
            }

            if (functionDef is ParameterExpression)
            {
                var genericFuncType = StaticFuncType(functionCallToken.Token.Length);

                functionDef = Expression.Convert(functionDef, genericFuncType);
            }

            return Expression.Invoke(functionDef,
                functionCallToken.Token.Select(Visit).Select(x => Expression.Convert(x, typeof(object))).ToList());
        }

        public override Expression Visit(FunctionDefToken functionDefToken)
        {
            _contour = _contour.Push();
            
            var formals = functionDefToken.Formals
                .Select(x => (Key: x, Formal: Expression.Parameter(typeof(object), x))).ToList();

            foreach (var (key, expression) in formals)
            {
                _contour.Add(key, expression);
            }

            var functionExpr = Expression.Lambda(Expression.Convert(Visit(functionDefToken.Body), typeof(object)), formals.Select(x => x.Formal));

            _contour = _contour.Pop();

            return _contour.Add(functionDefToken.Name, functionExpr);
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
                "^" =>  (ExpressionType.Power, typeof(double)),
                "==" => (ExpressionType.Equal, typeof(object)),
                "!=" => (ExpressionType.NotEqual, typeof(object)),
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

        private static Type StaticFuncType(int count)
        {
            return count switch
            {
                0 => typeof(Func<>).MakeGenericType(typeof(object)),
                1 => typeof(Func<,>).MakeGenericType(typeof(object), typeof(object)),
                2 => typeof(Func<,,>).MakeGenericType(typeof(object), typeof(object), typeof(object)),
                3 => typeof(Func<,,,>).MakeGenericType(typeof(object), typeof(object), typeof(object), typeof(object)),
                _ => throw new Exception(
                    $"function with {count} many formals is not supported")
            };
        }
    }
}