from flask import Flask, jsonify, request
from Engine.Search import SpotifySearch
from Engine.Session import Session

app = Flask(__name__)

@app.route("/song/<string:song_name>", methods=["GET"])
def add_song(song_name):
    session.add_based_from_song(SpotifySearch().find_song_by_name(song_name))
    return "Success"

@app.route("/tempo/increase", methods=["GET"])
def increase_tempo():
    session.increase_tempo()
    return "Tempo increased"

@app.route("/tempo/decrease", methods=["GET"])
def decrease_temp():
    session.decrease_tempo()
    return "Tempo decreased"

if __name__ == "__main__":
    session = Session()
    app.run(debug = True)
