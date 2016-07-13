// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Engine.Vote
open Engine.Music
open Engine.API
open Engine.ExportAPI
open System.Threading
open System
open FSharp.Data
open Engine.Session
open Engine.MusicSpace

open Engine.Spotify

[<EntryPoint>]
let main argv = 

    let starting_song = Engine.Spotify.search_song_by_name "Knights of Cydonia"
    printfn "%A" starting_song
    let track = 
        match starting_song with 
            | Success result -> 
                printfn "%A" result
                match result with
                    | Track track -> track

                         
    let position = Engine.Spotify.get_audio_features track
    let area = Engine.MusicSpace.create_uniform_dispersion_area  position Engine.MusicSpace.Dispersion.Medium

    Session.Add (NewArea area)

    Session.Add (TrackSuggestion (position, track))

    let rec new_recommendations previous_song = 
        let new_tracks = Engine.Spotify.generate_suggestions 1 previous_song
 
        new_tracks
            |> List.iter (fun new_t -> 
                let position = Engine.Spotify.get_audio_features new_t
                printfn "%A" position.key
                Session.Add (TrackSuggestion (position, new_t))
                System.Threading.Thread.Sleep(5000)
            )
            
        new_recommendations new_tracks.[0]
    new_recommendations track
    System.Console.Read() |> ignore
    printfn "%A" argv
    0 