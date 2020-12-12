using System.Linq;
using Core.Abstracts;
using Core.Interfaces;
using Core.Tokens;
using FParsec;
using FParsec.CSharp;
using Microsoft.FSharp.Core;
using static FParsec.CSharp.CharParsersCS;
using static FParsec.CSharp.PrimitivesCS;

namespace Core
{
    public class LispParser : StaticConstructor<LispParser>
    {
        private readonly FSharpFunc<CharStream<Unit>, Reply<IToken>> _expressionP;

        public Reply<IToken> Parse(string code)
        {
            return _expressionP.ParseString(code);
        }
        
        public LispParser()
        {
            FSharpFunc<CharStream<Unit>, Reply<IToken>> recP = null;

            var nameP = Many1Chars(NoneOf(new []{ '"', ' ', '(', ')'})).Label("name");

            var variableP = Many1Chars(CharP(char.IsLetter)).Label("variable").Map(x => (IToken) new ParameterToken(x));
            var stringP = Between(CharP('"'), nameP, CharP('"')).Label("string")
                .Map(x => (IToken) new StringToken(x));

            var numberP = Int.Lbl("number").Map(x => (IToken) new NumberToken(x));

            var nullP = StringP("null").Lbl("null").Return((IToken) new NullToken());

            var atomicP = Choice(numberP, stringP, nullP, variableP).Label("atomic");

            var conditionalP = StringP("if").AndL(WS1)
                .AndR(Rec(() => recP)).AndL(WS1)
                .And(Rec(() => recP)).AndL(WS1)
                .And(Rec(() => recP))
                .Label("conditional")
                .Map(x => (IToken) new ConditionalToken(x.Item1.Item1, x.Item1.Item2, x.Item2));

            var binaryOperatorP = Choice("<=", ">=", "<", ">", "==", "+", "-", "*", "/").And(WS1)
                .And(Rec(() => recP)).And(WS1)
                .And(Rec(() => recP))
                .Label("binaryOperator")
                .Map(x => (IToken) new BinaryOperatorToken(x.Item1.Item1, x.Item1.Item2, x.Item2));
            
            var uniaryOperatorP = Choice("!", "-").And(WS1)
                .And(Rec(() => recP))
                .Label("unaryOperator")
                .Map(x => (IToken) new UnaryToken(x.Item1, x.Item2));

            var functionCallP = nameP.AndL(WS1).And(Many(Rec(() => recP).AndL(WS))).Label("functionCall")
                .Map(x => (IToken) new FunctionCallToken(x.Item1, (x.Item2 ?? Enumerable.Empty<IToken>()).ToArray()));

            var functionDefP = StringP("defun").AndR(WS1)
                .AndR(nameP).AndL(WS1)
                .And(Between(CharP('('), Many(WS.AndR(nameP.AndL(WS))), CharP(')'))).AndL(WS1)
                .And(Rec(() => recP))
                .Label("functionDef")
                .Map(x => (IToken) new FunctionDefToken(x.Item1.Item1,
                    (x.Item1.Item2 ?? Enumerable.Empty<string>()).ToArray(),
                    x.Item2));

            _expressionP = Between(
                CharP('('),
                Choice(binaryOperatorP, uniaryOperatorP, conditionalP, functionDefP, functionCallP),
                CharP(')')
            );

            recP = atomicP.Or(_expressionP);
        }
    }
}