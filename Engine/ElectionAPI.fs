namespace Engine
open System
open FSharp.Data
open Engine.Music
open Engine.Vote

module API = 


    type SlackHistory = JsonProvider<"""{
    "ok": true,
    "messages": [
        {
            "type": "message",
            "user": "U0KUC87A4",
            "text": "Song - Jakes song",
            "ts": "1468233942.000005",
            "reactions": [
                {
                    "name": "disappointed",
                    "users": [
                        "U0KUC87A4"
                    ],
                    "count": 1
                },
                {
                    "name": "open_mouth",
                    "users": [
                        "U0KUC87A4"
                    ],
                    "count": 1
                }
            ]
        },
        {
            "type": "message",
            "user": "U0KUC87A4",
            "text": "Artist - Muse",
            "ts": "1468232852.000004",
            "reactions": [
                {
                    "name": "slightly_smiling_face",
                    "users": [
                        "U0KUC87A4"
                    ],
                    "count": 1
                }
            ]
        },
        {
            "user": "U04AWRADB",
            "inviter": "U0KUC87A4",
            "text": "<@U04AWRADB|jacob> has joined the channel",
            "type": "message",
            "subtype": "channel_join",
            "ts": "1468232828.000003"
        },
        {
            "user": "U0KUC87A4",
            "text": "<@U0KUC87A4|jakecoltman> has joined the channel",
            "type": "message",
            "subtype": "channel_join",
            "ts": "1468232827.000002"
        }
    ],
    "has_more": false
}""">

    let (|Prefix|_|) (p:string) (s:string) =
        if s.StartsWith(p) then
            Some(s.Substring(p.Length))
        else
            None

    let parse_to_entity message = 
        match message with 
            | Prefix "Album" rest -> Some(Album rest)
            | Prefix "Song" rest -> Some(Song rest)
            | Prefix "Artist" rest -> Some(Artist rest)
            | Prefix "Tempo" rest -> match rest with 
                                        | " increase" -> Some(Tempo Up)
                                        | " decrease" -> Some(Tempo Down)
                                        | _ -> None
            | _ -> None

    let extract_votes (message : SlackHistory.Message) = 

        let emoticon_to_position emoticon = 
            match emoticon with 
                | "slightly_smiling_face" -> Some Engine.Vote.InFavour
                | "disappointed" -> Some Engine.Vote.Opposed
                | _ -> None

        let reaction_to_votes (reaction : SlackHistory.Reaction): List<Vote> = 
            let name = reaction.Name
            let position : VoteDirection option = emoticon_to_position name
            match position with 
            | Some pos -> 
                reaction.Users 
                    |> List.ofArray
                    |> List.map (fun (x:string) -> x, pos)
            | None -> []

        message.Reactions
            |> Array.map (fun x -> reaction_to_votes x)
            |> List.ofArray
            |> List.concat

    let parse_message_to_election (message : SlackHistory.Message) = 
        let entity = parse_to_entity message.Text
        match entity with 
            | Some ent -> 
                extract_votes message
                    |> Engine.Vote.new_election ent
                    |> Some
            | None -> None
     
    let is_message_finished_election (message : SlackHistory.Message) = 
        message.Reactions
            |> Array.exists (fun x -> x.Name = "new_moon")
                
    let mark_all_elections_finished (messages) = 
        messages
            |> List.ofArray
            |> List.filter (fun x -> not( is_message_finished_election x))
            |> List.iter(fun x -> 
                            printfn "%A" x.Ts
                            let url = String.Format("https://slack.com/api/reactions.add?token={0}&channel=C1QG4RGNM&name=new_moon&timestamp={1}&pretty=1", System.Environment.GetEnvironmentVariable("SLACK_KEY"), x.Ts.ToString()) 
                            Http.RequestString(url) |> printfn "%A"
            )


    let get_active_elections (input) =
        printfn "%A" input 
        let token = System.Environment.GetEnvironmentVariable("SLACK_KEY")
        let url_string = @"https://slack.com/api/channels.history?token={0}&channel=C1QG4RGNM&pretty=1"
        let resp = Http.RequestString(String.Format(url_string, token))
        let parsed = SlackHistory.Parse(resp)

        mark_all_elections_finished parsed.Messages

        parsed.Messages
            |> List.ofArray
            |> List.filter (fun x -> not( is_message_finished_election x))
            |> List.map parse_message_to_election
            |> List.choose id