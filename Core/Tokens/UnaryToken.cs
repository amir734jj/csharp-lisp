using Core.Interfaces;

namespace Core.Tokens
{
    public class UnaryToken : IToken
    {
        public string Op { get; }
        
        public IToken Expr { get; }

        public UnaryToken(string op, IToken expr)
        {
            Op = op;
            Expr = expr;
        }
        
        public override string ToString()
        {
            return $"({Op} {Expr})";
        }
    }
}