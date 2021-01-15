(println (if (! (< 3 5)) "hooman" "amir"))

;; factorial
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(println (concat "fact 12 is: " (fact 12)))

;; fibonacci
(defun fib (x) (if (<= x 0) 0 (if (<= x 1) 1 (+ (fib (- x 1)) (fib (- x 2))))))
(println (concat "fib 12 is: " (fib 12)))

(println (concat "Aref: " (+ 3 5)))

(defun sigma (x) (if (<= x 0) 0 (+ x (sigma (- x 1))))) 
(defun pi (x) (if (<= x 1) 1 (* x (pi (- x 1)))))
(defun fn1 (l) (l 3))
(defun fn2 (l x) (l x))
(fn1 fact)
(fn2 fact 3)
(fn2 fact (fn2 fib 6))
(sigma 5)
(pi 5)
