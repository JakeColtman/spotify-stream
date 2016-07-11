// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Engine.Vote
open Engine.Music
open Engine.API
open Engine.PlayQueue

[<EntryPoint>]
let main argv = 
  

    let Jake : User = "U0KUC87A4"
    let user_weights : UserWeights = Map.empty.Add(Jake, 1.0)

    let elections = get_active_elections
    elections
        |> List.iter (fun x -> 
            let result = run_election x user_weights
            match result with 
                | Success(entity , weight)-> playqueue.Post entity
                | Rejection x -> ignore x
            )

    System.Console.Read() |> ignore
    printfn "%A" argv
    0 // return an integer exit code
