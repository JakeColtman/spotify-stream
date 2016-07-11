import spotipy
import spotipy.util as util
import json
from datetime import datetime

class Session:

    def __init__(self):
        with open("keys.json") as file_open:
            keys = json.load(file_open)
        scope = 'playlist-modify-public'
        token = util.prompt_for_user_token(keys["username"], scope)
        self.sp = spotipy.Spotify(auth=token)
        self.playlist = self.sp.user_playlist_create("jakecoltman", "PartyTime - {0}".format(datetime.now().isoformat()))
        self.id = self.playlist["id"]

    def add_songs(self, track_ids):
        results = self.sp.user_playlist_add_tracks("jakecoltman", self.id, track_ids)
        print(results)

session = Session()
session.add_songs(["7ouMYWpwJ422jRcDASZB7P"])