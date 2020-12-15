using Core.Tokens;

namespace Core.Interfaces
{
    public interface IVisitor<out T>
    {
        T Visit(BinaryOperatorToken binaryOperatorToken);
        
        T Visit(NumberToken numberToken);

        T Visit(ConditionalToken conditionalToken);

        T Visit(UnaryToken unaryToken);

        T Visit(FunctionDefToken functionDefToken);

        T Visit(FunctionCallToken functionCallToken);

        T Visit(ParameterToken parameterToken);

        T Visit(StringToken stringToken);
    }
}