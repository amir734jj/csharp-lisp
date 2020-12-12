using Core.Interfaces;

namespace Core.Tokens
{
    public class FunctionDefToken : IToken
    {
        public string Name { get; }
        
        public string[] Formals { get; }
        
        public IToken Body { get; }

        public FunctionDefToken(string name, string[] formals, IToken body)
        {
            Name = name;
            Formals = formals;
            Body = body;
        }
    }
}