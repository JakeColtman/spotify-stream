from flask import Flask, jsonify, request
from Engine.Search import SpotifySearch

app = Flask(__name__)

@app.route("/song/<string:song_name>", methods=["GET"])
def add_song(song_name):
    return SpotifySearch().find_song_by_name(song_name)

if __name__ == "__main__":
    #app.run(debug = True)
    print(add_song("Knights of Cydonia"))