module Vector

type Vector = { X: int; Y : int }

let create x y = { X = x; Y = y }

let add v1 v2 = { X = v1.X + v2.X; Y = v1.Y + v2.Y }

// let (+) (v1: Vector) (v2: Vector): Vector = add v1 v2
