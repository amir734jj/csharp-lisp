using Core.Interfaces;

namespace Core.Tokens
{
    public class ConditionalToken : IToken
    {
        public IToken CondExpr { get; }
        
        public IToken IfExpr { get; }
        
        public IToken ElseExpr { get; }

        public ConditionalToken(IToken condExpr, IToken ifExpr, IToken elseExpr)
        {
            CondExpr = condExpr;
            IfExpr = ifExpr;
            ElseExpr = elseExpr;
        }
        
        public override string ToString()
        {
            return $"(if {CondExpr} {IfExpr} {ElseExpr})";
        }
    }
}