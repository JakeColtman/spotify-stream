// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Engine.Vote
open Engine.Music
open Engine.API

[<EntryPoint>]
let main argv = 
    
    let Jake : User = "U0KUC87A4"
    let user_weights : UserWeights = Map.empty.Add(Jake, 1.0)

    let elections = get_active_elections
    elections
        |> List.iter (fun x -> 
            match x with 
                | Some els -> 
                    let result = run_election els user_weights
                    printfn "%A" result
                | None -> printfn "%A" "Not an election"
            )

    System.Console.Read() |> ignore
    printfn "%A" argv
    0 // return an integer exit code
