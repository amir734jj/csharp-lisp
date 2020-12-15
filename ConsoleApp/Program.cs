using System;
using Core;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string code = @"
(if (! (< 3 5)) ""hooman"" ""amir"")
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(defun fib (x) (if (<= x 0) 0 (if (<= x 1) 1 (+ (fib (- x 1)) (fib (- x 2))))))
(defun sigma (x) (if (<= x 0) 0 (+ x (sigma (- x 1))))) 
(defun pi (x) (if (<= x 1) 1 (* x (pi (- x 1)))))
(defun fn1 (l) (l 3))
(defun fn2 (l x) (l x))
(fn1 fact)
(fn2 fact 3)
(fn2 fact (fn2 fib 6))
(sigma 5)
(pi 5)
";

            Console.WriteLine(LipsSimulator.New().Simmulate<double>(code)());
        }
    }
}