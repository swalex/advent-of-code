module SolutionsManager

open System.Reflection
open System.Text.RegularExpressions

type Solution = {
    Day: int
    Solution1: string array array -> int
    Solution2: string array array -> int
}

let toIntMatrix (matrix: string array array) : int array array =
    matrix |> Array.map (Array.map int)

let wrapSolution (method: MethodInfo) : string array array -> int =
    fun input ->
        let paramType = method.GetParameters().[0].ParameterType
        let convertedInput =
            if paramType = typeof<int array array> then
                box (toIntMatrix input)
            else
                box input
        method.Invoke(null, [| convertedInput |]) :?> int

let discoverSolutions () =
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
