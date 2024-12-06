open FileReader
open SolutionsManager
open StringExtensions

let computeSolutionTimed (solution: string array -> int)(input: string array) : int * int64 =
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    let result = solution input
    let elapsed = stopwatch.ElapsedMilliseconds
    result, elapsed

let computeSolutionByKind (solution: Solution, kind: Kind) =
    let data = FileReader.readInputMatrix solution.Day kind
    let kindName = kindAsString kind |> capitalize
    match data with
    | Input.NotFound message ->
        printfn "%s" message
    | Input.Success input ->
        let result1, elapsed1 = computeSolutionTimed solution.Solution1 input
        printfn "%5s solution 1: %d (in %d ms)" kindName result1 elapsed1
        let result2, elapsed2 = computeSolutionTimed solution.Solution2 input
        printfn "%5s solution 2: %d (in %d ms)" kindName result2 elapsed2

let computeSolution (solution: Solution) =
    computeSolutionByKind(solution, Test)
    computeSolutionByKind(solution, Input)

[<EntryPoint>]
let main argv =
    printfn "Advend of Code 2024"

    let solutions = SolutionsManager.solutions.Value
    if solutions.Length = 0 then
        printfn "No solutions available."
        1
    else
        printfn "%d solutions available." solutions.Length

        match argv with
            | [| dayString |] ->
                match System.Int32.TryParse(dayString) with
                | (true, day) when day > 0 ->
                    let selected = solutions |> Array.tryFind (fun s -> s.Day = day)
                    match selected with
                    | Some solution ->
                        printfn "Computing solution for day %d..." day
                        computeSolution solution
                    | None ->
                        printfn "No solution found for day %d." day
                | _ ->
                    printfn "Invalid input '%s'. Please provide a valid day number." dayString
            | _ ->
                let latest = solutions |> Array.last
                printfn "Computing latest solution for day %d..." latest.Day
                computeSolution latest
        0
