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
        let summation = 
            election.votes
            |> List.map (fun (x : Vote) -> 
                match x with
                    | user, InFavour -> weights.[user]
                    | user, Opposed -> - weights.[user]
                )
            |> List.reduce (fun x y -> x + y)


        match summation with 
            | i when i <= 0.0 -> Rejection weights
            | _ -> Success (election.candidate, weights)


    let run_election (election: Election) (weights : UserWeights) = 
        election_result election weights
