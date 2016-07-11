namespace Engine
open Engine.Music

module Vote = 

    type User = string

    type UserWeights = Map<User, float>

    type VoteDirection = 
        | InFavour
        | Opposed

    type Vote = User * VoteDirection

    type ElectionResult = 
        | Success of MusicEntity * UserWeights
        | Rejection of UserWeights

    type Election = {
        candidate: MusicEntity;
        votes: List<Vote>
    }

    let new_election (entity :Engine.Music.MusicEntity) (votes : List<Vote>)  = 
        {candidate = entity; votes = votes}

    let vote_in_election (election: Election) (vote: Vote) = 
        {election with votes = vote::election.votes}

    let election_result (election: Election) (weights : UserWeights) = 

        let new_users = 
            election.votes
                |> List.map(fun vote -> fst vote)
                |> List.filter(fun user -> weights.ContainsKey user)
                |> List.map(fun user -> user, 1.0)   
            
        let new_weights = 
            weights 
                |> Map.toList 
                |> List.append new_users
                |> Map.ofList

        let summation = 
            election.votes
            |> List.map (fun (x : Vote) -> 
                match x with
                    | user, InFavour -> new_weights.[user]
                    | user, Opposed -> - new_weights.[user]
                )
            |> List.reduce (fun x y -> x + y)

        let result = 
            match summation with 
                | i when i <= 0.0 -> Rejection new_weights
                | _ -> Success (election.candidate, new_weights)

        result
        
    let run_election (election: Election) (weights : UserWeights) = 
        election_result election weights
