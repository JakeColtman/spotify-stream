namespace Engine

module Music = 

    type Song = string
    type Artist = string
    type Album = string

    type MusicEntity = 
        | Song of Song
        | Artist of Artist
        | Album of Album

module Requests = 

    type SpecificRequest = 
        | Song of string
        | Album of string
        | Artist of string
 
    type ThemeRequest = 
        | Genre of string
        | Tempo of string
        
    type Request = 
        | SpecificRequest of SpecificRequest
        | ThemeRequest of ThemeRequest

