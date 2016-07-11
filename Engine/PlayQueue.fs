namespace Engine 

module PlayQueue = 

    open Engine.Music
    open System.Collections

    type PlayQueue = {
        queue: Queue
    };

    let add_song (queue : PlayQueue) (song : Song) = 
        queue.queue.Enqueue(song)
        queue

    let get_next_song (queue : PlayQueue) = 
        let song = queue.queue.Dequeue
        song, queue

