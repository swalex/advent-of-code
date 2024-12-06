module Day01

let solution1(input: int array array): int =
    let leftColumn, rightColumn =
        input
        |> Array.fold (fun (left, right) row -> (row.[0]::left, row.[1]::right)) ([], [])

    let sortedLeft = leftColumn |> List.toArray |> Array.sort
    let sortedRight = rightColumn |> List.toArray |> Array.sort

    Array.zip sortedLeft sortedRight
    |> Array.map (fun (l, r) -> abs(r - l))
    |> Array.sum

let solution2(input: int array array): int =
    0
