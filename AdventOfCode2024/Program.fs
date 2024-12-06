open FileReader
open SolutionsManager

let computeSolutionTimed(solution: string array array -> int)(input: string array array): int * int64 =
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    let result = solution input
    let elapsed = stopwatch.ElapsedMilliseconds
    result, elapsed

let computeSolutionWithInput(solution: Solution, data: Input) =
    match data with
    | Input.NotFound message ->
        printfn "%s" message
        printfn "No input data available for day %d." solution.Day
    | Input.Success input ->
        let result1, elapsed1 = computeSolutionTimed solution.Solution1 input
        printfn "Solution 1: %d (in %d ms)" result1 elapsed1
        let result2, elapsed2 = computeSolutionTimed solution.Solution2 input
        printfn "Solution 2: %d (in %d ms)" result2 elapsed2

let computeSolution(solution: Solution) =
    let testInput = FileReader.readInputMatrix solution.Day Test
    computeSolutionWithInput(solution, testInput)
    let actualInput = FileReader.readInputMatrix solution.Day Input
    computeSolutionWithInput(solution, testInput)

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
