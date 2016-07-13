namespace Engine

module Spotify = 
    
    let auth = SpotifyAPI.Web.Auth.AutorizationCodeAuth
    let spotify = new SpotifyAPI.Web.SpotifyWebAPI()
    spotify.AccessToken <- "INSERT ACCESS TOKEN HERE"
    spotify.UseAuth <- true
    spotify.TokenType <- "Bearer"

    let username = "jakecoltman"

    type Track = 
        {id: string; uri : string}

    type SpotifyContent = 

        | Track of Track
        | Artist of string
        | Playlist of string

    type Result = 
        | Success of SpotifyContent
        | Failure

    let search_song_by_name (name: string) = 
        let resp = spotify.SearchItems(name, SpotifyAPI.Web.Enums.SearchType.Track)
        if resp.HasError() then 
            printfn "%A" resp.Error.Message
            Failure
        else Success (Track ({id= resp.Tracks.Items.[0].Id; uri = resp.Tracks.Items.[0].Uri}))

    let search_artist_by_name (name: string) = 
        let resp = spotify.SearchItems(name, SpotifyAPI.Web.Enums.SearchType.Artist)
        if resp.HasError() then Failure
        else Success (Artist(resp.Tracks.Items.[0].Id))

    let create_playlist = 
        let playlist = spotify.CreatePlaylist(username, System.DateTime.Now.ToLongTimeString())
        if playlist.HasError() then Failure
        else 
            printfn "%A" playlist.Id
            Success(Playlist(playlist.Id))

    let add_track_to_playlist (playlist_id: string) (track : Track) = 
        let result = spotify.AddPlaylistTrack(username, playlist_id, track.uri)
        if result.HasError() then printfn "%A" result.Error.Message
        else printfn "%A" "Success"

    let get_audio_features (track: Track) = 
        let result = spotify.GetAudioFeatures(track.id)
        Engine.MusicSpace.create_position result.Tempo result.Key result.Energy

    let generate_suggestions (number: int) (track : Track) = 
        let suggestion_list = new System.Collections.Generic.List<string>()
        suggestion_list.Add(track.id) |> ignore
        let result = spotify.GetRecommendations(trackSeed = suggestion_list)
        result.Tracks.GetRange(0,number)
            |> Seq.map (fun x -> {id = x.Id; uri = x.Uri})
            |> List.ofSeq