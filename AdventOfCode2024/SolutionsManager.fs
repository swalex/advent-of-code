module SolutionsManager

open System.Reflection
open System.Text.RegularExpressions

type Solution = {
    Day: int
    Solution1: string array -> int
    Solution2: string array -> int
}

let private toIntMatrix (matrix: string array array) : int array array =
    matrix |> Array.map (Array.map int)

let private toStringMatrix (input: string array) : string array array =
    input
    |> Seq.map (fun line -> line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries))
    |> Seq.toArray

let private wrapSolution (method: MethodInfo) : string array -> int =
    fun input ->
        let paramType = method.GetParameters().[0].ParameterType
        let convertedInput =
            if paramType = typeof<int array array> then
                box (toStringMatrix input |> toIntMatrix)
            else if paramType = typeof<string array array> then
                box (toStringMatrix input)
            else if paramType = typeof<string> then
                box (String.concat "" input)
            else
                box input
        method.Invoke(null, [| convertedInput |]) :?> int

let private discoverSolutions () =
    let assembly = Assembly.GetExecutingAssembly()
    let pattern = Regex("^Day(\d{2})$")
    assembly.GetTypes()
        |> Array.choose(fun t ->
            match pattern.Match(t.Name) with
            | m when m.Success ->
                let day = int m.Groups.[1].Value
                let solution1 = t.GetMethod("solution1", BindingFlags.Static ||| BindingFlags.Public)
                let solution2 = t.GetMethod("solution2", BindingFlags.Static ||| BindingFlags.Public)
                if solution1 <> null && solution2 <> null then
                    Some {
                        Day = day
                        Solution1 = wrapSolution solution1
                        Solution2 = wrapSolution solution2
                    }
                else
                    printfn "Missing solution methods for day %d." day
                    None
            | _ -> None)
        |> Array.sortBy (fun solution -> solution.Day)

let solutions = lazy(discoverSolutions())
