namespace Engine
open Engine.Music

module Vote = 

    type User = string

    type UserWeights = Map<User, float>

    type VoteDirection = 
        | InFavour
        | Opposed

    type Vote = User * VoteDirection

    type Election = {
        candidate: MusicEntity;
        votes: List<Vote>
    }

    type ElectionResult = 
        | Success of MusicEntity
        | Rejection

    let execute_election (election: Election) (weights : UserWeights) = 
        let summation = 
            election.votes
            |> List.map (fun (x : Vote) -> 
                match x with
                    | user, InFavour -> weights.[user]
                    | user, Opposed -> - weights.[user]
                )
            |> List.reduce (fun x y -> x + y)

        let result =
             match summation with 
                | i when i <= 0.0 -> Opposed
                | _ -> InFavour

        Success election.candidate
