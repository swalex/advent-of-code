module Day02

let private allDecreasing(input: int array): bool =
    input
    |> Array.pairwise
    |> Array.forall (fun (a, b) -> a > b)

let private allIncreasing(input: int array): bool =
    input
    |> Array.pairwise
    |> Array.forall (fun (a, b) -> a < b)

let private allWithinRange(input: int array): bool =
    let isWithinRange(distance: int): bool =
        distance >= 0 && distance <= 3
    input
    |> Array.pairwise
    |> Array.forall (fun (a, b) -> abs(a - b) |> isWithinRange)

let private isSafe(input: int array): bool =
    (allDecreasing input || allIncreasing input) && allWithinRange input

let private isDampenedSafe(input: int array): bool =
    seq {
        for i in 0..input.Length - 1 do
            input |> Array.mapi (fun j x -> if j = i then None else Some x) |> Array.choose id
    }
    |> Seq.exists isSafe

let private isSafeOrDampenedSafe(input: int array): bool =
    isSafe input || isDampenedSafe input

let solution1 (input: int array array): int =
    input |> Array.filter isSafe |> Array.length

let solution2 (input: int array array): int =
    input |> Array.filter isSafeOrDampenedSafe |> Array.length
