namespace Engine

module MusicSpace = 

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

    type Probability = float

    type ProbabilityFunction = 
        MusicPosition -> Probability

    type MusicArea = {
        centre : MusicPosition;
        prob_density_function : ProbabilityFunction
    }

    type MusicFilter = {
        areas: MusicArea list
    }

    let range_dispersion (dispersion: Dispersion) = 
        match dispersion with 
            | Specific -> 10
            | Narrow -> 20
            | Medium -> 50
            | Wide -> 100

    let squared_difference (pos1: float32) (pos2: float32) = 
        (pos1 - pos2) ** (float32 2.0)

    let distance_between_points (first_point : MusicPosition) (second_point : MusicPosition) = 
        let tempo_difference = squared_difference first_point.tempo second_point.tempo
        let mood_difference = squared_difference first_point.mood second_point.mood

        (mood_difference + tempo_difference) ** (float32 0.5)

    let uniform_dispersion_probability_density (centre: MusicPosition) (dispersion: Dispersion) (point: MusicPosition) = 
        let distance_from_centre = distance_between_points centre point
        let area_range = range_dispersion dispersion
        if (distance_from_centre < float32 area_range) then 1.0
        else 0.0

    let position_in_filter (filter: MusicFilter) (position: MusicPosition) = 
        let prob_not_in = 
            filter.areas
                |> List.map(fun x -> 1.0 - x.prob_density_function position)
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

    let create_uniform_dispersion_area (position: MusicPosition) (dispersion: Dispersion) = 
        {
            centre = position;
            prob_density_function = uniform_dispersion_probability_density position dispersion
        }

    let add_area_to_filter filter area = 
        {filter with areas = area::filter.areas}
