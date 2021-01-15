using Core.Interfaces;

namespace Core.Tokens
{
    public class Comment : IToken
    {
        public string Value { get; }

        public Comment(string value)
        {
            Value = value;
        }
    }
}