# csharp-lisp

Attempting to write a toy LIPS compiler

- Supports:
- binary, unary, conditional, functions
- recursion

Does not support:
- first class functions

Library:
- FParsec
- C# Expression Tree for back-end
- Supporting Recursion

Example:
```
(defun f (x) (if (< x 3) (+ x 1) (+ (fact x) 12)))
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(f (fact 3))
```
