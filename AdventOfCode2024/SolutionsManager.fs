module SolutionsManager

open System.Reflection
open System.Text.RegularExpressions

type Solution = {
    Day: int
    Solution1: string array array -> int
    Solution2: string array array -> int
}

let discoverSolutions() =
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
                        Solution1 = (fun input -> solution1.Invoke(null, [| box input |]) :?> int)
                        Solution2 = (fun input -> solution2.Invoke(null, [| box input |]) :?> int)
                    }
                else
                    printfn "Missing solution methods for day %d." day
                    None
            | _ -> None)
        |> Array.sortBy (fun solution -> solution.Day)

let solutions = lazy(discoverSolutions())
