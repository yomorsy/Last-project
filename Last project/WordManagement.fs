module WordManagement

open System.IO
open System.Text.Json

// Load or initialize the dictionary
let loadDictionary (filePath: string) : Map<string, string> =
    if File.Exists filePath then
        let json = File.ReadAllText filePath
        JsonSerializer.Deserialize<Map<string, string>>(json)
    else
        Map.empty

let saveDictionary (filePath: string) (dictionary: Map<string, string>) =
    let json = JsonSerializer.Serialize(dictionary)
    File.WriteAllText(filePath, json)

// Initialize dictionary with predefined words
let ensurePredefinedWords (dict: Map<string, string>) : Map<string, string> =
    let predefinedWords: (string * string) list =
        [ 
            "apple", "A fruit that is red, green, or yellow in color."
            "fsharp", "A functional-first programming language on the .NET platform."
            "sun", "The star at the center of the solar system."
        ]
    predefinedWords
    |> List.fold (fun acc (word, definition) ->
        if not (Map.containsKey word acc) then
            Map.add word definition acc
        else acc) dict
