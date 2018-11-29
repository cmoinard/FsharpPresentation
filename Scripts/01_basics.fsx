// Types

let x = 12

// Products
type PointTuple = int * int
let point = 1, 2

type Point = {
    x: int
    y: int
}
let point' = { x = 3; y = 4 }

// Union
type Shape =
    | Point
    | Square of int
    | Rect of int * int

// Point
// Square 3
// Rect (3, 4)

// Fonctions

let shout message = message + "!!!"
shout "Hello"
"Hello" |> shout

let add x y = x + y

let increment = add 1
// add:  int -> (int -> int)
// incr:         int -> int

add 1 3
3 |> add 1