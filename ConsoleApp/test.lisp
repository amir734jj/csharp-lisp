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
(println (concat "pi approximation: " (pi 0 (^ 10 3))))

;; sum
(defun sigma (x) (if (<= x 0) 0 (+ x (sigma (- x 1))))) 
(println (concat "sigma of 5: " (sigma 5)))

;; array
(defun range (x y) (if (> x y) null (append (single x) (range (+ x 1) y))))
(println (concat "range [3..10]: " (range 3 5)))

;; map
(defun map (fn ls) (if (isNull ls) null (append (single (fn (head ls))) (map fn (tail ls)))))

(defun triple (x) (* x 3))
(println (concat "range 2x[3..10]: " (map triple (range 3 5))))

(return 1)