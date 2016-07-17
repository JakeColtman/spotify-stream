namespace Engine
open Engine.MusicSpace
open Engine.MusicSpaceAPI
open Engine.Spotify
#nowarn "40"
module Session = 

    type Message = 
        | NewArea of Engine.MusicSpace.Region
        | TrackSuggestion of Engine.MusicSpaceDomain.MusicPosition * Engine.Spotify.Track
        | Translation of Engine.MusicSpaceDomain.MusicSpaceAxis

    type Session () = 


        static let process_message music_filter playlist_id msg = 

            let music_api = Engine.MusicSpaceAPI.standard_api
            try
                printfn "Starting tempo %A" music_filter.areas.[0].centre.tempo
            with 
                | _ -> printfn "%A" "No tracks"
            match msg with 
                | NewArea x -> 
                    printfn "%A" "Adding a new area"
                    music_api.add_area_to_filter music_filter x
                | TrackSuggestion (pos, track) -> 
                    printfn "%A" "Processing a new track suggestion"
                    printfn "%A" track
                    if (position_in_filter music_filter pos)
                        then 
                        printfn "%A" pos.tempo
                        printfn "%A" music_filter.areas.[0].centre.tempo
                        Engine.Spotify.add_track_to_playlist playlist_id track

                    else
                        printfn "%A" "not in filter"
                    music_filter
                | Translation axis ->
                    printfn "%A" "Translating"
                    let new_filter = music_api.translate_filter music_filter axis
                    printfn "Translated tempo %A" new_filter.areas.[0].centre.tempo
                    new_filter

        static let agent = MailboxProcessor.Start(fun inbox -> 

            let rec messageLoop music_filter playlist_id = async{        
                let! msg = inbox.Receive()
                let new_filter = process_message music_filter playlist_id msg
                try
                    printfn "Message loop temp %A" new_filter.areas.[0].centre.tempo
                with 
                    | _ -> printfn "%A" "no tacks"
                System.Threading.Thread.Sleep(2000)
                return! messageLoop new_filter playlist_id   
            }

            let new_playlist = Engine.Spotify.create_playlist
            match new_playlist with 
                | Success playlist -> 
                    match playlist with 
                        | Playlist id -> 
                            messageLoop create_filter id
            )

        // public interface to hide the implementation
        static member Add i = agent.Post i