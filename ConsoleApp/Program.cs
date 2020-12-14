using System;
using Core;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string code = @"
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(defun fn1 (l) (l 3))
(defun fn2 (l x) (l x))
(fn1 fact)
(fn2 fact 3)
";

            Console.WriteLine(LipsSimulator.New().Simmulate<double>(code)());
        }
    }
}