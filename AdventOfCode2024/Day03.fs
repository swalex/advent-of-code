module Day03

open System.Text.RegularExpressions

let private expression = Regex("mul\((\d{1,3}),(\d{1,3})\)")
let private expressionWithSwitches = Regex("(?:mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\))")

type private Operation = {
    X: int
    Y: int
}

let private parseOperation (m: Match) : Operation =
    {
        X = int m.Groups.[1].Value
        Y = int m.Groups.[2].Value
    }

let private runOperation (o: Operation) : int =
    o.X * o.Y

let private filterBySwitches (matches: Match seq): Match seq =
    matches
    |> Seq.scan (fun (isOn, _) m ->
        match m.Value with
        | "do()" -> (true, None)
        | "don't()" -> (false, None)
        | _ when isOn -> (true, Some m)
        | _ -> (false, None)) (true, None)
    |> Seq.choose snd

let solution1 (input: string): int =
    expression.Matches(input)
    |> Seq.map parseOperation
    |> Seq.map runOperation
    |> Seq.sum

let solution2 (input: string): int =
    expressionWithSwitches.Matches(input)
    |> filterBySwitches
    |> Seq.map parseOperation
    |> Seq.map runOperation
    |> Seq.sum
