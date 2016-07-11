import spotipy
import spotipy.util as util
import json
from datetime import datetime
from Engine.Exploration import Proposer
from Engine.Chooser import Chooser

class Session:

    def __init__(self, proposer = Proposer(), chooser = Chooser()):
        self.proposer = proposer
        self.chooser = chooser
        with open("keys.json") as file_open:
            keys = json.load(file_open)
        scope = 'playlist-modify-public'
        token = util.prompt_for_user_token(keys["username"], scope)
        self.sp = spotipy.Spotify(auth=token)
        self.playlist = self.sp.user_playlist_create("jakecoltman", "PartyTime - {0}".format(datetime.now().isoformat()))
        self.id = self.playlist["id"]

    def add_based_from_song(self, track_id):
        proposed_track_ids = self.proposer.propose_from_track(track_id)
        chosen_track_ids = self.chooser.choose(proposed_track_ids)
        results = self.sp.user_playlist_add_tracks("jakecoltman", self.id, chosen_track_ids)

    def increase_tempo(self):
        self.chooser.change_target()

    def decrease_tempo(self):
        self.chooser.change_target()