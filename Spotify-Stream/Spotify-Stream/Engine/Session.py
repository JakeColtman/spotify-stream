from Engine.Exploration import Proposer
from Engine.Chooser import Chooser
from Engine.Playlist import Playlist


class Session:
    def __init__(self, proposer=Proposer(), chooser=Chooser(), playlist=Playlist()):
        self.proposer = proposer
        self.chooser = chooser
        self.playlist = playlist

    def add_based_from_song(self, track_id):
        proposed_track_ids = self.proposer.propose_from_track(track_id)
        chosen_track_ids = self.chooser.choose(proposed_track_ids)
        chosen_track_ids = [track_id] + chosen_track_ids
        self.playlist.add_tracks(chosen_track_ids)

    def more_of_same(self):
        track_id = self.playlist.last_track
        self.add_based_from_song(track_id)

    def increase_tempo(self):
        # self.chooser.change_target()
        print("temp increase")

    def decrease_tempo(self):
        print("temp decrease")
        # self.chooser.change_target()
