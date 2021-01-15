# csharp-lisp

Toy LIPS compiler that converts one-liner S-Expressions to C# expression trees

Supports:
- binary, unary, conditional, functions
- recursion
- first-class functions
- shadowing of variables thanks to contours

TODO:
- ~~unary/binary expression (-) is ambiguous for the parser combinator~~ 
- better error handling in the paster
- ~add standard libary support (e.g. `Console.log(...)`)~

Libraries:
- FParsec for parsing
- C# Expression Tree for the back-end of compiler
- ExpressionTreeToString for debugging
- built-in functions: `print`, `println`, `concat`

Example:
given the following, `LipsSimulator` class will return the result of last line:

```lisp
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(defun fib (x) (if (<= x 0) 0 (if (<= x 1) 1 (+ (fib (- x 1)) (fib (- x 2))))))
(defun fn1 (l) (l 3))
(defun fn2 (l x) (l x))
(fn1 fact)
(fn2 fact 3)
;; will print 6
```

Note:
- LISP is untyped but C# expression trees are typed. To make things work, I used `object` and `Func<object+>` everyone until compiler encounters conditional, binary/unary operation or functions as parameters. At that point it tries to apply necessary conversion to make things type-check.
