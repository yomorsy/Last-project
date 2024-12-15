module SearchFunction

/// Searches the dictionary for entries that match the given keyword (case-insensitive).
let searchWord (dictionary: Map<string, string>) (keyword: string) : (string * string) list =
    let lowerKeyword = keyword.ToLower()
    dictionary
    |> Map.toSeq
    |> Seq.filter (fun (key, value) -> 
        key.ToLower().Contains(lowerKeyword) || value.ToLower().Contains(lowerKeyword))
    |> Seq.toList
