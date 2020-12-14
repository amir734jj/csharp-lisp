using Core.Interfaces;

namespace Core.Tokens
{
    public class BinaryOperatorToken : IToken
    {
        public string Op { get; }
        
        public IToken Expr1 { get; }
        
        public IToken Expr2 { get; }

        public BinaryOperatorToken(string op, IToken expr1, IToken expr2)
        {
            Op = op;
            Expr1 = expr1;
            Expr2 = expr2;
        }

        public override string ToString()
        {
            return $"({Op} {Expr1} {Expr2})";
        }
    }
}