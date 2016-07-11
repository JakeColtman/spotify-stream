import spotipy


class SpotifySearch:
    def __init__(self):
        self.spot = spotipy.Spotify()

    def find_song_by_name(self, name):
        print(name)
        song = self.spot.search(name, type="track")
        return song["tracks"]["items"][0]["id"]

    def find_album_by_name(self, name):
        return self.spot.search(name, type="artist")

    def find_artist_by_name(self, name):
        return self.spot.search(name, type="album")
