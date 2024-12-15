module Main

open System
open System.Windows.Forms
open System.Drawing
open WordManagement
open SearchFunction

// File path for saving the dictionary
let dictionaryFilePath = @"C:\Users\asus\Last project\dictionary.json"

// Initialize the dictionary
let mutable dictionary =
    let loadedDict = loadDictionary dictionaryFilePath
    let updatedDict = ensurePredefinedWords loadedDict
    saveDictionary dictionaryFilePath updatedDict
    updatedDict

// GUI Components
let form = new Form(Text = "Digital Dictionary", Width = 700, Height = 500, BackColor = Color.LightBlue)

// Welcome label
let lblWelcome = new Label(Text = "Welcome to the Digital Dictionary!", 
                           Font = new Font("Arial", 16.0F, FontStyle.Bold), 
                           ForeColor = Color.DarkBlue,
                           Left = 150, Top = 10, Width = 400, Height = 30)

// Word input
let lblWord = new Label(Text = "Word:", Font = new Font("Arial", 12.0F), Left = 10, Top = 60, Width = 60)
let txtWord = new TextBox(Left = 80, Top = 60, Width = 200)

// Definition input
let lblDefinition = new Label(Text = "Definition:", Font = new Font("Arial", 12.0F), Left = 10, Top = 100, Width = 80)
let txtDefinition = new TextBox(Left = 100, Top = 100, Width = 400)

// Buttons
let btnAddUpdate = new Button(Text = "Add/Update", BackColor = Color.LightGreen, Left = 10, Top = 140, Width = 100)
let btnDelete = new Button(Text = "Delete", BackColor = Color.IndianRed, Left = 120, Top = 140, Width = 100)
let btnSearch = new Button(Text = "Search", BackColor = Color.CornflowerBlue, Left = 230, Top = 140, Width = 100)

// Search results
let lblSearchResult = new Label(Text = "Search Results:", Font = new Font("Arial", 12.0F), Left = 10, Top = 200, Width = 150)
let lstSearchResult = new ListBox(Left = 10, Top = 230, Width = 660, Height = 200, Font = new Font("Consolas", 10.0F))

// Event Handlers
btnAddUpdate.Click.Add(fun _ ->
    let word = txtWord.Text.Trim()
    let definition = txtDefinition.Text.Trim()
    if word <> "" && definition <> "" then
        dictionary <- Map.add (word.ToLower()) definition dictionary
        saveDictionary dictionaryFilePath dictionary
        MessageBox.Show("Word added/updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        txtWord.Clear()
        txtDefinition.Clear()
    else
        MessageBox.Show("Please enter both word and definition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)

btnDelete.Click.Add(fun _ ->
    let word = txtWord.Text.Trim()
    if word <> "" then
        let key = word.ToLower()
        if Map.containsKey key dictionary then
            dictionary <- Map.remove key dictionary
            saveDictionary dictionaryFilePath dictionary
            MessageBox.Show("Word deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        else
            MessageBox.Show("Word not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        txtWord.Clear()
    else
        MessageBox.Show("Please enter a word to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)

btnSearch.Click.Add(fun _ ->
    let keyword = txtWord.Text.Trim()
    lstSearchResult.Items.Clear()
    if keyword <> "" then
        let results = searchWord dictionary keyword
        if results.Length > 0 then
            for (word, definition) in results do
                lstSearchResult.Items.Add(box $"{word}: {definition}") |> ignore
        else
            lstSearchResult.Items.Add(box "No results found.") |> ignore
    else
        MessageBox.Show("Please enter a word or keyword to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)

// Add components to form
form.Controls.AddRange([| lblWelcome; lblWord; txtWord; lblDefinition; txtDefinition; btnAddUpdate; btnDelete; btnSearch; lblSearchResult; lstSearchResult |])

// Run the application
Application.Run(form)
