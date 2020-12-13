using System;
using Core;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string code = @"
(defun f (x) (if (< x 3) (+ x 1) (+ (fact x) 12)))
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(defun negate (x) (f x))
(defun fn (l) (l 3))
(fn f)
";

            Console.WriteLine(LipsSimulator.New().Simmulate<double>(code)());
        }
    }
}