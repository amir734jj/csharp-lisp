using Core.Interfaces;

namespace Core.Tokens
{
    public class NullToken : IToken
    {
        public override string ToString()
        {
            return "null";
        }
    }
}