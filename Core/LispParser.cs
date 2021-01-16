using System.Linq;
using Core.Interfaces;
using Core.Tokens;
using FParsec;
using FParsec.CSharp;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using static FParsec.CSharp.CharParsersCS;
using static FParsec.CSharp.PrimitivesCS;

namespace Core
{
    public class LispParser
    {
        public readonly FSharpFunc<CharStream<Unit>, Reply<FSharpList<IToken>>> ExpressionsP;

        public LispParser()
        {
            FSharpFunc<CharStream<Unit>, Reply<IToken>> recP = null;

            var nameP = Many1Chars(NoneOf(new []{ '"', ' ', '(', ')'})).Label("name");

            var variableP = Many1Chars(CharP(char.IsLetter)).Label("variable").Map(x => (IToken) new ParameterToken(x));
            var stringP = Between(CharP('"'), ManyChars(NoneOf(new []{'"'})).Label("stringPValue"), CharP('"')).Label("string")
                .Map(x => (IToken) new StringToken(x));

            var numberP = Int.Lbl("number").Map(x => (IToken) new NumberToken(x));

            var nullP = StringP("null").Lbl("null").Return((IToken) new NullToken());

            var atomicP = Choice(numberP, stringP, nullP, variableP).Label("atomic");
            
            var commentP = StringP(";;").AndR(ManyChars(NoneOf(new[] {'\n'}))).Map(x => (IToken) new Comment(x));

            var conditionalP = StringP("if").AndL(WS1)
                .AndR(Rec(() => recP)).AndL(WS1)
                .And(Rec(() => recP)).AndL(WS1)
                .And(Rec(() => recP))
                .Label("conditional")
                .Map(x => (IToken) new ConditionalToken(x.Item1.Item1, x.Item1.Item2, x.Item2));

            var binaryOperatorP = Choice("<=", ">=", "<", ">", "==", "+", "-", "*", "/", "^").AndTry(WS1)
                .AndTry(Rec(() => recP)).AndTry(WS1)
                .AndTry(Rec(() => recP))
                .Label("binaryOperator")
                .Map(x => (IToken) new BinaryOperatorToken(x.Item1.Item1, x.Item1.Item2, x.Item2));
            
            var unaryOperatorP = Choice("!", "-").AndLTry(WS1)
                .AndTry(Rec(() => recP))
                .Label("unaryOperator")
                .Map(x => (IToken) new UnaryToken(x.Item1, x.Item2));

            var functionCallP = nameP.AndL(WS1).And(Opt(Many(Rec(() => recP).AndL(WS)))).Label("functionCall")
                .Map(x => (IToken) new FunctionCallToken(x.Item1, (x.Item2 ?? Enumerable.Empty<IToken>()).ToArray()));

            var functionDefP = StringP("defun").AndRTry(WS1)
                .AndRTry(nameP).AndLTry(WS1)
                .AndTry(Between(CharP('('), Many(WS.AndR(nameP.AndL(WS))), CharP(')'))).AndL(WS1)
                .AndTry(Rec(() => recP))
                .Label("functionDef")
                .Map(x => (IToken) new FunctionDefToken(x.Item1.Item1,
                    (x.Item1.Item2 ?? Enumerable.Empty<string>()).ToArray(),
                    x.Item2));

            var expr = Between(
                CharP('('),
                Choice(binaryOperatorP, unaryOperatorP, conditionalP, functionDefP, functionCallP, atomicP),
                CharP(')')
            );

            ExpressionsP = WS.AndR(Many(Choice(expr, commentP), WS, true));

            recP = atomicP.Or(expr).Or(commentP);
        }
    }
}