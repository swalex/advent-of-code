module Day04

open Vector

let private xmas = [| 'X'; 'M'; 'A'; 'S' |]
let private x_mas = [| 'M'; 'A'; 'S' |]

let private isMatch (input: char[,]) (pattern: char[]) (offset: Vector) (direction: Vector) : bool =
    [0..pattern.Length - 1]
    |> Seq.forall (fun i -> input.[offset.Y + i * direction.Y, offset.X + i * direction.X] = pattern.[i])

let private horizontalIndices (input: char[,]) (pattern: char[]) (direction: Vector) : seq<int> =
    if direction.X = 0 then
        seq { 0..input.GetLength(1) - 1 }
    else if direction.X > 0 then
        seq { 0..input.GetLength(1) - pattern.Length }
    else
        seq { input.GetLength(1) - 1..-1..pattern.Length - 1 }

let private verticalIndices (input: char[,]) (pattern: char[]) (direction: Vector) : seq<int> =
    if direction.Y = 0 then
        seq { 0..input.GetLength(0) - 1 }
    else if direction.Y > 0 then
        seq { 0..input.GetLength(0) - pattern.Length }
    else
        seq { input.GetLength(0) - 1..-1..pattern.Length - 1 }

let private singleDirection = [-1; 0; 1]

let private allDirections =
    singleDirection
    |> Seq.collect (fun y -> singleDirection |> Seq.map (fun x -> create x y))
    |> Seq.filter (fun v -> v <> create 0 0)

let private getMatches (input: char[,]) (pattern: char[]) (direction: Vector) : seq<Vector> =
    verticalIndices input pattern direction
    |> Seq.collect (fun y ->
        horizontalIndices input pattern direction
        |> Seq.map (fun x -> create x y))
    |> Seq.filter (fun offset -> isMatch input pattern offset direction)

let solution1 (input: char[,]): int =
    allDirections
    |> Seq.map (fun direction -> getMatches input xmas direction)
    |> Seq.concat
    |> Seq.length

let solution2 (input: char[,]): int =
    let slash =
        [create 1 -1; create -1 1]
        |> Seq.collect (fun v -> getMatches input x_mas v |> Seq.map (fun m -> Vector.add m v))
    let backslash =
        [create 1 1; create -1 -1]
        |> Seq.collect (fun v -> getMatches input x_mas v |> Seq.map (fun m -> Vector.add m v))
        |> Set.ofSeq

    slash
    |> Seq.filter (fun v -> backslash |> Seq.exists (fun v' -> v = v'))
    |> Seq.length
