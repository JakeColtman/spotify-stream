namespace Engine

module Music = 

    type Song = string
    type Artist = string
    type Album = string
   
    type Direction = 
        | Up
        | Down

    type Tempo= Direction

    type MusicEntity = 
        | Song of Song
        | Artist of Artist
        | Album of Album
        | Tempo of Tempo 