namespace Engine

module Vote = 

    type User = string

    type UserWeights = Map<User, float>

    type VoteDirection = 
        | InFavour
        | Opposed

    type Vote = User * VoteDirection

    let execute_election (weights: UserWeights) (votes : List<Vote>) = 
        let summation = 
            votes
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

        result
