import spotipy
import spotipy.util as util


class Proposer:
    def __init__(self):
        token = util.prompt_for_user_token("jakecoltman")
        self.sp = spotipy.Spotify(auth=token)
        self.proposed = []

    def propose_from_track(self, track_id):
        propositions = self.sp.recommendations(seed_tracks=[track_id])
        print(propositions)
        prop_ids = [x["id"] for x in propositions["tracks"] if x["id"] not in self.proposed]
        self.proposed += prop_ids
        return prop_ids
