using Core.Interfaces;

namespace Core.Tokens
{
    public class NumberToken : IToken
    {
        public double Number { get; }

        public NumberToken(double number)
        {
            Number = number;
        }
        
        public override string ToString()
        {
            return $"{Number}";
        }
    }
}