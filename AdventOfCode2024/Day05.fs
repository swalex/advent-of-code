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

let private fixOrder (update: int[]) (rules: Set<Rule>) : int[] =
    let rec fix (currentEntries: int[]) (currentRules: Set<Rule>) : List<int> =
        let relevantRules = currentRules |> Set.filter (fun rule ->
            Array.contains rule.X update && Array.contains rule.Y update)

        let nextEntry =
            currentEntries
            |> Array.tryFind (fun x -> not(Seq.exists (fun rule -> rule.Y = x) relevantRules))
            |> Option.defaultValue currentEntries.[0]
            
        let remainingEntries =
            currentEntries |> Array.filter (fun x -> x <> nextEntry)

        let remainingRules =
            relevantRules |> Set.filter (fun rule -> rule.X <> nextEntry)

        if remainingEntries.Length = 0 then
            [nextEntry]
        else
            nextEntry::(fix remainingEntries remainingRules)
    
    fix update rules
    |> List.toArray

let private getMiddlePage(update: int[]) : int =
    update |> Array.item (update.Length / 2)

let private parseRules (input: string array) : Set<Rule> =
    input
    |> Array.filter (fun line -> line.Contains("|"))
    |> Array.map (fun line -> line.Split("|"))
    |> Array.map (fun parts -> { X = int parts.[0]; Y = int parts.[1] })
    |> Set.ofArray

let private parseUpdates (input: string array) : Update[] =
    input
    |> Array.filter (fun line -> line.Contains(","))
    |> Array.map (fun line -> line.Split(","))
    |> Array.map (fun parts -> parts |> Array.map int)

let solution1 (input: string array): int =
    let rules = parseRules input

    parseUpdates input
    |> Array.filter (fun update -> isCorrectlyOrdered update rules)
    |> Array.map getMiddlePage
    |> Array.sum

let solution2 (input: string array): int =
    let rules = parseRules input

    parseUpdates input
    |> Array.filter (fun update -> not(isCorrectlyOrdered update rules))
    |> Array.map (fun u -> fixOrder u rules)
    |> Array.map getMiddlePage
    |> Array.sum
