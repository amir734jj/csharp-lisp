using Core.Interfaces;

namespace Core.Tokens
{
    public class BoolLiteral : IToken
    {
        public bool Value { get; }

        public BoolLiteral(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString().ToLower();
        }
    }
}