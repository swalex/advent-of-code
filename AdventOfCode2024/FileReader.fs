module FileReader

open System.IO

type Kind =
    | Input
    | Test

let kindAsString = function
    | Input -> "input"
    | Test -> "test"

type Input =
    | Success of string array
    | NotFound of string

let readInputMatrix (day: int) (kind: Kind) : Input =
    let kindAsString = kindAsString kind
    let path = sprintf "input/%02d/%s.txt" day kindAsString
    if File.Exists(path) then
        Success (File.ReadAllLines(path))
    else
        NotFound (sprintf "%s file for day %d not found." kindAsString day)
