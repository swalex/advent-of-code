module Day01

let solution1(input: int array array): int =
    let leftColumn = input |> Array.map (fun row -> row.[0]) |> Array.sort
    let rightColumn = input |> Array.map (fun row -> row.[1]) |> Array.sort
    let distances = Array.zip leftColumn rightColumn |> Array.map (fun (l, r) -> abs(r - l))
    distances |> Array.sum

let solution2(input: int array array): int =
    0
