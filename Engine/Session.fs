namespace Engine
open Engine.MusicSpace
open Engine.Spotify
#nowarn "40"
module Session = 

    type Message = 
        | NewArea of Engine.MusicSpace.MusicArea
        | TrackSuggestion of Engine.MusicSpace.MusicPosition * Engine.Spotify.Track

    type Session () = 

        static let process_message music_filter playlist_id msg = 

            match msg with 
                | NewArea x -> 
                    printfn "%A" "Adding a new area"
                    Engine.MusicSpace.add_area_to_filter music_filter x
                | TrackSuggestion (pos, track) -> 
                    printfn "%A" "Processing a new track suggestion"
                    printfn "%A" "Track"
                    if (position_in_filter music_filter pos)
                        then Engine.Spotify.add_track_to_playlist playlist_id track
                    else
                        printfn "%A" "not in filter"
                    music_filter

        // create the agent
        static let agent = MailboxProcessor.Start(fun inbox -> 

            // the message processing function
            let rec messageLoop music_filter playlist_id = async{        
                let! msg = inbox.Receive()
                let new_filter = process_message music_filter playlist_id msg
                System.Threading.Thread.Sleep(1000)
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