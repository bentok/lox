module lox.Lox

open System

type Scanner(input: string) =
    member this.ScanTokens() =
        input.Split([| ' '; '\n'; '\t' |], StringSplitOptions.RemoveEmptyEntries)

let mutable hadError = false

let run source =
    let scanner = Scanner(source)
    let tokens = scanner.ScanTokens()

    for token in tokens do
        printfn "%s" token

let rec runPrompt () =
    printf "> "
    let line = Console.ReadLine() |> Option.ofObj

    match line with
    | None -> ()
    | Some l ->
        run l
        hadError <- false
        runPrompt ()

let runFile path =
    path
    |> System.IO.File.ReadAllBytes
    |> System.Text.Encoding.UTF8.GetString
    |> run

    if (hadError = true) then
        Environment.Exit 65

let report line where message =
    printfn "[line %d] Error %s: %s" line where message
    hadError <- true

let error line message = report line "" message

let init (args: string[]) =
    if args.Length > 1 then
        printfn "Usage: flox [script]"
        Environment.Exit 64
    elif args.Length = 1 then
        let file = args[0]
        runFile file
        Environment.Exit 0
    else
        runPrompt ()
        Environment.Exit 0
