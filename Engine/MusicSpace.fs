﻿namespace Engine

module MusicSpace = 

    type Tempo = float
    
    type Mood = float

    type KeyLetter = 
        | A
        | B
        | C
        | D
        | E
        | F
        | G

    type KeyLevel = 
        | Major 
        | Minor

    type Key = KeyLetter * KeyLevel

    type MusicPosition = {
        tempo: Tempo;
        mood: Mood;
        key: Key
    }

    type Dispersion = 
        | Specific
        | Narrow
        | Medium
        | Wide

    type MusicArea = {
        centre : MusicPosition;
        dispersion : Dispersion
    }

    let distance_tempo (first_point : MusicPosition) (second_point : MusicPosition) =  
        if (abs(first_point.tempo - second_point.tempo) < 1.0 )
            then Specific
        elif (abs(first_point.tempo - second_point.tempo) < 10.0 )
            then Narrow
        elif (abs(first_point.tempo - second_point.tempo) < 20.0 )
            then Medium
        else
            Wide
                
    let distance_key (first_point : MusicPosition) (second_point : MusicPosition) =  
        if first_point.key = second_point.key then Narrow
        else Wide

    let distance_mood (first_point : MusicPosition) (second_point : MusicPosition) =  
        //Holding function - to be filled out
        // Realistically all the continuous variables will get parameterized
        if (abs(first_point.mood - second_point.mood) < 1.0 )
            then Specific
        elif (abs(first_point.mood - second_point.mood) < 10.0 )
            then Narrow
        elif (abs(first_point.mood - second_point.mood) < 20.0 )
            then Medium
        else
            Wide

    let position_in_area (area : MusicArea) (point: MusicPosition) = 
        let distances =
            [
                distance_mood; distance_tempo; distance_key
            ]
            |> List.map(fun x -> x area.centre point)

        match area.dispersion with 
            | Narrow -> 
                List.exists (fun x -> match x with 
                                        | Narrow -> false
                                        | _ -> true) distances
                |> not
            | Specific -> 
                List.exists (fun x -> match x with 
                                        | Narrow -> false
                                        | Specific -> false
                                        | _ -> true) distances
            | Medium -> 
                List.exists (fun x -> match x with 
                                        | Narrow -> false
                                        | Specific -> false
                                        | Medium -> false
                                        | _ -> true) distances
                |> not
            | Wide -> true


