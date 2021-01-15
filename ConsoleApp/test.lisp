;; test
(println (if (! (< 3 5)) "hooman" "amir"))

;; factorial
(defun fact (x) (if (<= x 0) 1 (* x (fact (- x 1)))))
(println (concat "fact 12 is: " (fact 12)))

;; fibonacci
(defun fib (x) (if (<= x 0) 0 (if (<= x 1) 1 (+ (fib (- x 1)) (fib (- x 2))))))
(println (concat "fib 12 is: " (fib 12)))

;; pi
(defun pi (c k) (if (< k 0) (* c 4) (+ c (pi (/ (^ -1 k) (+ (* 2 k) 1)) (- k 1)))))
(println (concat "pi approximation: " (pi 0 (^ 10 4))))

;; sum
(defun sigma (x) (if (<= x 0) 0 (+ x (sigma (- x 1))))) 
(sigma 5)