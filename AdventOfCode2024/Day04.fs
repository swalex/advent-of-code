module Day04

open Vector

let private xmas = ['X'; 'M'; 'A'; 'S']

let private isMatch (input: char[,]) (offset: Vector) (direction: Vector) : bool =
    [0..3]
    |> Seq.forall (fun i -> input.[offset.Y + i * direction.Y, offset.X + i * direction.X] = xmas.[i])

let private horizontalIndices (input: char[,]) (direction: Vector) : seq<int> =
    if direction.X = 0 then
        seq { 0..input.GetLength(1) - 1 }
    else if direction.X > 0 then
        seq { 0..input.GetLength(1) - 4 }
    else
        seq { input.GetLength(1) - 1..-1..3 }

let private verticalIndices (input: char[,]) (direction: Vector) : seq<int> =
    if direction.Y = 0 then
        seq { 0..input.GetLength(0) - 1 }
    else if direction.Y > 0 then
        seq { 0..input.GetLength(0) - 4 }
    else
        seq { input.GetLength(0) - 1..-1..3 }

let private singleDirection = [-1; 0; 1]

let private directions =
    singleDirection
    |> Seq.collect (fun y -> singleDirection |> Seq.map (fun x -> create x y))
    |> Seq.filter (fun v -> v <> create 0 0)

let private countMatches (input: char[,]) (direction: Vector) : int =
    verticalIndices input direction
    |> Seq.collect (fun y ->
        horizontalIndices input direction
        |> Seq.map (fun x -> create x y))
    |> Seq.filter (fun offset -> isMatch input offset direction)
    |> Seq.length

let private countAllMatches (input: char[,]) : int =
    directions
    |> Seq.map (fun direction -> countMatches input direction)
    |> Seq.sum

let solution1 (input: char[,]): int =
    countAllMatches input

let solution2 (input: char[,]): int =
    0
