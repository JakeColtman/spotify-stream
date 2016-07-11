namespace Engine
module ExportAPI = 
    open Engine.Music
    open System 
    open FSharp.Data

    let export (entity : Engine.Music.MusicEntity) = 
        
        let request_url = 
            match entity with 
                | Tempo Up -> Some "http://localhost:5000/tempo/increase"
                | Tempo Down -> Some "http://localhost:5000/tempo/decrease"
                | Song x -> Some (String.Format("http://localhost:5000/song/{0}", x))
                | _ -> None

        match request_url with 
            | Some url -> Http.RequestString(url) |> ignore
            | None -> ignore request_url