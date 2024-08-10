module lox.Lox

open System

type Scanner(input: string) =
        member this.ScanTokens() =
            input.Split([|' '; '\n'; '\t'|], StringSplitOptions.RemoveEmptyEntries)

type Lox () =
    let mutable hadError = false
   
    member x.run source =
        let scanner = Scanner(source)
        let tokens = scanner.ScanTokens()
        
        for token in tokens do
            printfn "%s" token
            
    member x.runPrompt () =
        let rec runPrompt () =
            printf "> "
            let line = Console.ReadLine()
            match line with
            | null -> ()
            | _ ->
                x.run(line)
                hadError <- false
                x.runPrompt()
        runPrompt()

    member x.runFile path =
        path
        |> System.IO.File.ReadAllBytes
        |> System.Text.Encoding.UTF8.GetString
        |> x.run
        
        if (hadError = true) then
            Environment.Exit 65

    member x.Run (args: string[]) =
        if args.Length > 1 then
            printfn "Usage: flox [script]"
            64
        elif args.Length = 1 then
            let file = args[0]
            x.runFile file
            0
        else
            x.runPrompt()
            0
    
    member x.report line where message =
        printfn "[line %d] Error %s: %s" line where message
        hadError <- true

    member x.error line message =
        x.report line "" message