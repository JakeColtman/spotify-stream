// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Engine.Vote
open Engine.Music
open Engine.API
open Engine.ExportAPI
open System.Threading
open System

[<EntryPoint>]
let main argv = 
  

    let Jake : User = "U0KUC87A4"
    let user_weights : UserWeights = Map.empty.Add(Jake, 1.0)

    let counter = ref 0

    while true do 
         
        (get_active_elections counter)
            |> List.filter(fun x -> x.votes.Length > 0 )
            |> List.iter (fun x -> 
                let result = run_election x user_weights
                printfn "%A" result
               // match result with 
                 //   | Success(entity , weight)-> export entity
                   // | Rejection x -> ignore x
                )

        incr counter
        System.Threading.Thread.Sleep(6000)

    System.Console.Read() |> ignore
    printfn "%A" argv
    0 // return an integer exit code
