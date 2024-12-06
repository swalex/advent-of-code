module SolutionsManager

open System.Reflection
open System.Text.RegularExpressions

type Solution = {
    Day: int
    Solution1: string array -> int
    Solution2: string array -> int
}

let private toCharArray2D (input: string array) : char[,] =
    let rows = input.Length
    let cols = input.[0].Length
    let result = Array2D.zeroCreate<char> rows cols
    for i in 0..rows - 1 do
        for j in 0..cols - 1 do
            result.[i, j] <- input.[i].[j]
    result

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
            match paramType with
            | _ when paramType = typeof<int array array> -> box (toStringMatrix input |> toIntMatrix)
            | _ when paramType = typeof<string array array> -> box (toStringMatrix input)
            | _ when paramType = typeof<string> -> box (String.concat "" input)
            | _ when paramType = typeof<char[,]> -> box (toCharArray2D input)
            | _ -> box input
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
