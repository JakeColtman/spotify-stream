// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Engine.Vote
open Engine.Music

[<EntryPoint>]
let main argv = 
    
    let Jake : User = "Jake"
    let user_weights : UserWeights = Map.empty.Add(Jake, 1.0)
    
    let jake_vote = Jake, Engine.Vote.Opposed

    let election = {candidate = Song "Jakes song"; votes = [jake_vote]}

    printfn "%A" (execute_election election user_weights)

    System.Console.Read() |> ignore
    printfn "%A" argv
    0 // return an integer exit code
