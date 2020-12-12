using Core.Interfaces;

namespace Core.Tokens
{
    public class StringToken : IToken
    {
        public string Str { get; }

        public StringToken(string str)
        {
            Str = str;
        }
    }
}