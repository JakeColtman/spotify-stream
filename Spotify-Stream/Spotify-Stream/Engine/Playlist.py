import spotipy.util as util
import json
from datetime import datetime
import spotipy


class Playlist:
    def __init__(self):
        with open("keys.json") as file_open:
            keys = json.load(file_open)
        scope = 'playlist-modify-public'
        token = util.prompt_for_user_token(keys["username"], scope)
        self.sp = spotipy.Spotify(auth=token)
        self.playlist = self.sp.user_playlist_create("jakecoltman",
                                                     "PartyTime - {0}".format(datetime.now().isoformat()))
        self.id = self.playlist["id"]

        self.last_track = "1hu7wC5M5RvEsKQ6P2AXps"

    def track_count(self):
        return len(self.sp.user_playlist("jakecoltman", self.id)["tracks"])

    def add_tracks(self, track_ids):
        self.sp.user_playlist_add_tracks("jakecoltman", self.id, track_ids)
        self.last_track = track_ids[-1]