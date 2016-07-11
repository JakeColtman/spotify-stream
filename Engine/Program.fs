// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Engine.Vote
open Engine.Music

[<EntryPoint>]
let main argv = 
    
    let Jake : User = "Jake"
    let user_weights : UserWeights = Map.empty.Add(Jake, 1.0)
    
    let jake_vote = Jake, Engine.Vote.Opposed

    let result = run_election (Song "Jake Song") [jake_vote] user_weights

    printfn "%A" result

    System.Console.Read() |> ignore
    printfn "%A" argv
    0 // return an integer exit code
