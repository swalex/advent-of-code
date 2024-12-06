module Day01

let splitToColumns (input: int array array): int array * int array =
    input
    |> Array.fold (fun (left, right) row -> (row.[0]::left, row.[1]::right)) ([], [])
    |> fun (left, right) -> (left |> List.toArray, right |> List.toArray)

let solution1 (input: int array array): int =
    let leftColumn, rightColumn = splitToColumns input

    let sortedLeft = leftColumn |> Array.sort
    let sortedRight = rightColumn |> Array.sort

    Array.zip sortedLeft sortedRight
    |> Array.map (fun (l, r) -> abs(r - l))
    |> Array.sum

let solution2 (input: int array array): int =
    let leftColumn, rightColumn = splitToColumns input

    let similarity (entry: int): int =
        rightColumn
        |> Array.filter (fun r -> r = entry)
        |> Array.length
        |> fun count -> count * entry

    leftColumn
    |> Array.map similarity
    |> Array.sum
