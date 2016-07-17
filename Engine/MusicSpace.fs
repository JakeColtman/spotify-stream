namespace Engine

module MusicSpaceDomain = 

    type Tempo = float32
    
    type Period = 
        | Early
        | Late

    type Mood = float32

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

    type Key = int

    type MusicSpaceAxis = 
        | Tempo of Tempo
        | Mood of Mood

    type MusicPosition = {
        tempo: Tempo;
        mood: Mood;
        key: Key
    }

    


module Distributions = 

    open MusicSpaceDomain

    type Probability = float

    type ProbabilityFunction = 
        MusicPosition -> MusicPosition -> Probability

    type Dispersion = 
        | Specific
        | Narrow
        | Medium
        | Wide

    let range_dispersion (dispersion: Dispersion) = 
        match dispersion with 
            | Specific -> 5
            | Narrow -> 20
            | Medium -> 50
            | Wide -> 100

    let uniform_dispersion_probability_density distance_function (dispersion: Dispersion) (centre: MusicPosition) (point: MusicPosition) = 
        let distance_from_centre = distance_function centre point
        let area_range = range_dispersion dispersion
        if (distance_from_centre < float32 area_range) then 1.0
        else 0.0

    let move_position_in_axis (point: MusicPosition) (axis: MusicSpaceAxis) = 
        match axis with 
            | Tempo amount -> {point with tempo = point.tempo + amount}
            | Mood amount -> {point with mood = point.mood + amount}        

module MusicSpace = 
    open MusicSpaceDomain
    open Distributions

    type Region = {
        centre : MusicPosition;
        prob_density_function : ProbabilityFunction
    }

    type Filter = {
        areas: Region list
    }

    let squared_distance_along_axis (pos1: float32) (pos2: float32) = 
        (pos1 - pos2) ** (float32 2.0)

    let euclidean_distance (first_point : MusicPosition) (second_point : MusicPosition) = 
        let tempo_difference = squared_distance_along_axis first_point.tempo second_point.tempo
        let mood_difference = squared_distance_along_axis first_point.mood second_point.mood

        ((mood_difference + tempo_difference) ** (float32 0.5))

    let position_in_filter (filter: Filter) (position: MusicPosition) = 
        let prob_not_in = 
            filter.areas
                |> List.map(fun x -> 1.0 - x.prob_density_function x.centre position)
                |> List.reduce (fun x y -> x * y)

        if 1.0 - prob_not_in > 0.5
            then true
        else
            false 

    let create_position (tempo: float32) (key: int) (mood: float32) = 
        {
            tempo = tempo;
            mood = mood;
            key= key
        }

    let create_filter = 
        {
            areas = []
        }

    let translate_points_in_filter (filter: Filter) (axis: MusicSpaceAxis) = 
        let new_points = filter.areas
                            |> List.map(fun x -> {x with centre = move_position_in_axis x.centre axis})
        {filter with areas = new_points}

    let create_uniform_dispersion_area (position: MusicPosition) (dispersion: Dispersion) = 
        {
            centre = position;
            prob_density_function = uniform_dispersion_probability_density euclidean_distance dispersion
        }

    let add_area_to_filter filter area = 
        {filter with areas = area::filter.areas}

module MusicSpaceAPI = 

    open MusicSpace
    open MusicSpaceDomain
    open Distributions

    type API ={
        new_session: Filter
        add_area_to_filter: Filter -> Region -> Filter
        translate_filter: Filter -> MusicSpaceAxis -> Filter
    }

    let standard_api = 
    {
        new_session = create_filter;
        add_area_to_filter = add_area_to_filter;
        translate_filter = translate_points_in_filter
    }
    