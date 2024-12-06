module FileReader

open System
open System.IO

type Kind =
    | Input
    | Test

let kindAsString = function
    | Input -> "input"
    | Test -> "test"

type Input =
    | Success of string array array
    | NotFound of string

let readInputMatrix(day: int)(kind: Kind): Input =
    let kindAsString = kindAsString kind
    let path = sprintf "input/day%02d/%s.txt" day kindAsString
    if File.Exists(path) then
        let lines =
            File.ReadAllLines(path)
            |> Seq.map (fun line -> line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            |> Seq.toArray
        Success lines
    else
        NotFound (sprintf "%s file for day %d not found." kindAsString day)
