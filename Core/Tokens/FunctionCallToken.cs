using System.Linq;
using Core.Interfaces;

namespace Core.Tokens
{
    public class FunctionCallToken : IToken
    {
        public string Name { get; }
        
        public IToken[] Token { get; }

        public FunctionCallToken(string name, params IToken[] token)
        {
            Name = name;
            Token = token;
        }
        
        public override string ToString()
        {
            return $"({Name} {string.Join(' ', Token.Select(x => x.ToString()))})";
        }
    }
}