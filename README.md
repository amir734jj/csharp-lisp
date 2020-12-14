# csharp-lisp

Toy LIPS compiler that converts S-Expressions to C# expression trees

Supports:
- binary, unary, conditional, functions
- recursion
- first-class functions
- shadowing of variables thanks to contours

TODO:
- unary/binary expression (-) is ambiguous for the parser combinator 
- better error handling in the paster

Library:
- FParsec for parsing
- C# Expression Tree for the back-end of compiler
- ExpressionTreeToString for debugging 

Example:

```lisp
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(defun fn1 (l) (l 3))
(defun fn2 (l x) (l x))
(fn1 fact)
(fn2 fact 3)
```
