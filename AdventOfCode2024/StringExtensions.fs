module StringExtensions

let capitalize(s: string) =
    if System.String.IsNullOrEmpty(s) then s
    else s.[0..0].ToUpper() + s.[1..].ToLower()
