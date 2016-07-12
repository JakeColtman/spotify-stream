namespace Engine
open Engine.MusicSpace
#nowarn "40"
module Session = 
    
    type Message = 
        | NewArea of Engine.MusicSpace.MusicArea
        | TrackSuggestion of Engine.MusicSpace.MusicPosition

    let session = MailboxProcessor.Start(fun inbox-> 
        let mutable music_filter = Engine.MusicSpace.create_filter

        // the message processing function
        let rec messageLoop = async{
        
            // read a message
            let! msg = inbox.Receive()
        
            match msg with 
                | NewArea x -> 
                    printfn "%A" "Adding a new area"
                    music_filter <- Engine.MusicSpace.add_position_to_filter music_filter x.centre x.dispersion
                | TrackSuggestion x -> 
                    printfn "%A" "Processing a new track suggestion"
                    printfn "%A" "Track"
                    printfn "%A" (position_in_filter music_filter x)

            // loop to top
            return! messageLoop  
        }

        // start the loop 
        messageLoop 
    )