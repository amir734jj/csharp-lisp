using Core.Interfaces;

namespace Core.Tokens
{
    public class ParameterToken : IToken
    {
        public string Name { get; }

        public ParameterToken(string name)
        {
            Name = name;
        }
        
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}