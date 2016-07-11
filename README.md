# Spotify Stream

Spotify is a fantastic tool for controlling music in situations where there is a single controller, single listener.  The user/listener can choose specific tracks they like or setup automated playlists and be reasonably sure that they'll enjoy the music.  

However, this doesn't scale to situations where there are multiple listeners.  Either you have a single controller who plays music the room might not like, or everyone can control and the music flow becomes eclectic and poorly structure.

Enter Spotify Stream

The tool integrates with Slack (or really any mass communication tool) and aggregates the preferences of everyone at the event.  People can collecitvely vote on the kind of music that should be playing.  

This voting focusses around the overall flow of music rather than individual tracks to create a superior outcome.  The tool will play a mix of songs centred around a specific point in property space.  e.g. Songs that have a similar key, mood and tempo.  People then vote to change the poisition in property space.  For example, there can be a vote on increasing the tempo, lightening the mood and so on.

Of course, often one finds that it is easy to think of songs you'd like to hear but hard to express this in terms of tempo and beat.  To resovle this, songs can be used as proxies for certain positions in property space.  People might vote to switch to Dvorak's Symphony No. 3 and this will (in addition to playing said Sympthony) switch the tool to focus on instrumental songs and increase the tempo.

### Extensions

The architecture is specifically designed to decouple the voting from the music selection.  This allows the tool to be extended to any source of changes to position in music space:

* Link to accelerometer to increase tempo as you run faster on jogs
* Link to GPS to create computer game different musical themes for different areas
* Link to motion camera to play more livly music if people are moving around more
* Link to light sensors to switch to smooth jazz as it goes dark
 
Some of these may make there way into the main tool through time :)
