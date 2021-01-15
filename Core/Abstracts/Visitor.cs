using System;
using Core.Interfaces;
using Core.Tokens;

namespace Core.Abstracts
{
    public abstract class Visitor<T> : IVisitor<T>
    {
        public virtual T Visit(UnaryToken unaryToken)
        {
            throw new NotImplementedException();
        }

        public virtual T Visit(BinaryOperatorToken binaryOperatorToken)
        {
            throw new NotImplementedException();
        }

        public virtual T Visit(NumberToken numberToken)
        {
            throw new NotImplementedException();
        }

        public virtual T Visit(ConditionalToken conditionalToken)
        {
            throw new NotImplementedException();
        }
        
        public virtual T Visit(FunctionDefToken functionDefToken)
        {
            throw new NotImplementedException();
        }
        
        public virtual T Visit(FunctionCallToken functionCallToken)
        {
            throw new NotImplementedException();
        }

        public virtual T Visit(ParameterToken parameterToken)
        {
            throw new NotImplementedException();
        }

        public virtual T Visit(StringToken stringToken)
        {
            throw new NotImplementedException();
        }

        public virtual T Visit(Comment stringToken)
        {
            throw new NotImplementedException();
        }

        protected T Visit(IToken token)
        {
            return token switch
            {
                UnaryToken unaryToken => Visit(unaryToken),
                BinaryOperatorToken binaryOperatorToken => Visit(binaryOperatorToken),
                NumberToken numberToken => Visit(numberToken),
                ConditionalToken conditionalToken => Visit(conditionalToken),
                FunctionDefToken functionDefToken => Visit(functionDefToken),
                FunctionCallToken functionCallToken => Visit(functionCallToken),
                ParameterToken parameterToken => Visit(parameterToken),
                StringToken stringToken => Visit(stringToken),
                Comment comment => Visit(comment),
                _ => throw new ArgumentOutOfRangeException(nameof(token))
            };
        }
    }
}