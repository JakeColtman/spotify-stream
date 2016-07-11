namespace Engine 
#nowarn "40"
module PlayQueue = 

    open Engine.Music
    open System.Collections


    let playqueue = MailboxProcessor.Start(fun inbox  -> 

        let rec messageLoop = async{
            let! (msg : Engine.Music.MusicEntity) = inbox.Receive()
            printfn "playing: %A" msg
            return! messageLoop  
            }

        // start the loop 
        messageLoop 
        )
//
//    type PlayQueue = {
//        queue: Queue
//    };
//
//    let add_song (queue : PlayQueue) (song : Song) = 
//        queue.queue.Enqueue(song)
//        queue
//
//    let get_next_song (queue : PlayQueue) = 
//        let song = queue.queue.Dequeue
//        song, queue

