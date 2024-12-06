module Day05

type private Rule = {
    X: int
    Y: int
}

type private Update = int[]

let private isCorrectlyOrdered (update: int[]) (rules: Set<Rule>) : bool =
    not (Seq.exists (fun i ->
        let x = update.[i]
        Seq.exists (fun j ->
            let y = update.[j]
            let rule = { X = y; Y = x }
            rules.Contains rule
        ) [i + 1..update.Length - 1]
    ) [0..update.Length - 2])

let private getMiddlePage(update: int[]) : int =
    update |> Array.item (update.Length / 2)

let solution1 (input: string array): int =
    let rules = input
                |> Array.filter (fun line -> line.Contains("|"))
                |> Array.map (fun line -> line.Split("|"))
                |> Array.map (fun parts -> { X = int parts.[0]; Y = int parts.[1] })
                |> Set.ofArray

    input
    |> Array.filter (fun line -> line.Contains(","))
    |> Array.map (fun line -> line.Split(","))
    |> Array.map (fun parts -> parts |> Array.map int)
    |> Array.filter (fun update -> isCorrectlyOrdered update rules)
    |> Array.map getMiddlePage
    |> Array.sum

let solution2 (input: string array): int =
    0
