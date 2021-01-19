;; test
(println (if (! (< 3 5)) "hello" "world"))

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
(println (concat "range [3..5]: " (range 3 5)))

;; map
(defun map (fn ls) (if (isNull (head ls)) null (append (single (fn (head ls))) (map fn (tail ls)))))
(println (concat "range 3x [3..5]: " (map (defun triple (x) (* x 3)) (range 3 5))))

;; filter
(defun filter (fn ls) (if (isNull (head ls)) null (append (if (fn (head ls)) (single (head ls)) null) (filter fn (tail ls)))))
(println (concat "filter range [-3..5] >= 0: " (filter (defun positive (x) (>= x 0)) (range -3 5))))

;; reduce
(defun reduce (fn ls acc) (if (isNull (head ls)) acc (reduce fn (tail ls) (fn acc (head ls)))))
(println (concat "Any grater than zero: " (reduce (defun anyPositiveR (acc x) (if acc true (> x 0))) (range -3 -1) false)))

;; any
(defun any (fn ls) (reduce (defun anyF (acc x) (if acc true (fn x))) ls false))
(println (concat "Any grater than zero: " (any (defun anyPositive (x) (> x 0)) (range -3 1))))

;; all
(defun all (fn ls) (reduce (defun allF (acc x) (if acc false (fn x))) ls true))
(println (concat "All grater than zero: " (all (defun allPositive (x) (> x 0)) (range -3 1))))

;; reverse
(defun reverse (ls) (if (isNull (head ls)) null (append (reverse (tail ls)) (single (head ls)))))
(println (concat "Reverse of [1..10]: " (reverse (range 1 10))))

;; find
(defun find (fn ls) (head (filter fn ls)))
(println (concat "Find 1 in [1..10]: " (find (defun findF (x) (== x 1)) (range 1 10))))

;; nth
(defun nth (i ls) (if (isNull (head ls)) null (if (== i 0) (head ls) (nth (- i 1) (tail ls)))))
(println (concat "nth 1 in [1..10]: " (nth 3 (range 1 10))))

(identity 1)
