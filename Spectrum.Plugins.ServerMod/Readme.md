#### __!! Spectrum.Plugins.ServerMod works for stable versions 5081 !!__  

Spectrum.Plugins.ServerMod is a plugin that adds some commands to the server, accessible by the host and players.  
All the commands are prefixed by '!'.  
If you are a client and you have the plugin, you can use some of your commands by remplacing the '!' key to '%'  

Commands have 3 permission levels:
* __HOST__: Only the host can use the command
* __ALL__: All players can use the command
* __LOCAL__: Client-side command

Some commands with __ALL__ permission level can also be used as a client.  

__Playlist managing commands can't be used on trackmogrify mode (trackmogrify don't use playlist).__ 

# Command list (alphabetized):

#### Auto:
Permission: __HOST__  
Use: !auto  
Toggle the server auto mode.  
When auto mode is activated, the server will automatically jump to the next level when all players finish.  
If a level lasts longer than 15 minutes, the server continues to the next level.  
If the playlist ends, the server shuffle the playlist automatically.  
Auto mode doesn't work with Trackmogrify.  
At the end of a map, if votes are enabled, players can choose between the 3 next maps (or restart the current) on the playlist the one they wants to play next.

#### Autospec:
Permission: __LOCAL__  
Use: !autospec  
Toggles automatic spectating (useful when you go AFK or use auto mode).  
If you are the host and no players are online, the server will automatically return to the lobby.

#### Clear:
Permission: __HOST__  
Use: !clear  
Removes everything on the playlist.

#### Countdown:
Permission: __HOST__  
Use: !countdown  
Starts the default end-of-race countdown (60 seconds).  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!countdown [time]  
Starts the end-of-race countdown with a time between 10 and 300 seconds.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!countdown stop  
Stops the current countdown (including the default game-initiated countdown).

#### Date
Permission: __ALL__ (can also be used as client)  
Use: !date  
Shows the date and time of the owner of the plugin.

#### Del:
Permission: __HOST__  
Use: !del [index]  
Removes the map at the entered index for the current playlist.  
The next map has an index of 1.

#### ForceStart:
Permission: __HOST__  
Use: !forcestart  
Forces the game to start regardless of the ready states of players in the lobby. Use with caution!

#### Help:
Permission: __ALL__ (can also be used as client)  
Use: !help  
Shows all the available commands.  
Commands with "(H)" can only be used by the host.  
Commands with "(L)" can only be used by the local player (the one who has the plugin).  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!help [command name]  
Shows the help message of the specified command.

#### Level:
Permission: __ALL__  (can also be used as client)  
Use: !level [keyword]  
Shows all levels in the host’s library that contain the entered keyword.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!level [filter]  
Shows all levels that match the filter.  
See bellow for more information about filters.  
If this command is used as a client with __%level__, it search on your library.

#### List:
Permission: __ALL__ (can also be used as client)  
Use: !list  
Shows all connected clients and their IDs

#### Load:
Permission: __HOST__  
Use: !load  
Shows all the available playlists.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!load [playlist name]  
Removes all the levels on the current playlist and load a new one.  
If you are not on the lobby, the current level is added at the start of the new playlist.
    
#### Play:
Permission: __HOST__  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;__ALL__ (if players can add maps)  
Use: !play [name]  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!play [filter]  
Adds a level to the playlist in the first position (the next level to be played)  
If more than one level matches the name exactly, the first one is added.  
If only one level contains the name, it will be added to the playlist.  
If more than one level contains the name, it will display the matching levels (like !level).  
The filter works exactly like the list command.

#### Playlist:
Permission: __ALL__  
Use: !playlist  
Shows the 10 next levels in the playlist.  
The first one is the current level.

#### Plugin:
Permission: __ALL__  
Use: !plugin  
Shows all the players that have server mod installed and the version.  

#### Rip:
Permission: __ALL__  
Use: !rip  
Prints a losing sentence.

#### Save:
Permission: __HOST__  
Use: !save [name]  
Save the current playlist on a file on the game playlist directory.  
It can be loaded agains with the __!load__ command, or on the lobby.  

#### Scores:
Permission: __ALL__ (can also be used as client)  
Use: !scores  
Shows the in-game “Show Scores” list in the chat. In racing modes, it shows the distance to the finish for each player. In Stunt, it shows scores in eV, and in Reverse Tag, it shows each player’s bubble possession time.

#### Server:
Permission: __ALL__ (without parameters) - (can also be used as client)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;__HOST__ (with parameters)  
Use: !server  
Shows the current server name.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!server private [password]  
Sets the server private with the given password.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!server public  
Sets the server public.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!server [name]  
Changes the name of the server.

#### Settings:
Permission: __HOST__  
Use: !settings reload  
Reloads the settings from file.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!settings vote [true/false]  
Allows players to vote for the next map on auto mode.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!settings play [true/false]  
Allows players to add maps on the playlist with the __!play__ command.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!settings addOne [true/false]  
If players can add maps and this option enabled, the players can only add one map at a time .
    
#### Shuffle:
Permission: __HOST__  
Use: !shuffle  
Shuffles the current playlist.  Already-played levels will also be shuffled.  
This command resets the playlist index, placing the current level at the beginning of the playlist.

#### Spec:
Permission: __HOST__  
Use: !spec [id]  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!spec [player name]  
Forces a player to spectate the game.

#### TimeLimit
Permission: __HOST__  
Use: !timelimit [value]  
Works like the official command, it changes the max time for the next reverse tag maps.  
The value must be between 30 and 1800 seconds (30 minutes)

#### Win
Permission: __ALL__  
Use: !win  
Prints a random winning sentence.

# Filters
Commands __level__ and __play__ can use filters to allow you to search maps.
* __-name__ or __-n__ : Select maps that contains theses words on it's name.
* __-author__ or __-a__ : Only show you maps form the entred author 
* __-mode__ or __-m__ : Select map on one particular gamemode. If not specified, it use the current gamemode
    * Valid gamemodes are : sprint, challenge, tag, soccer, style, stun  
    _(Soccer mode don't work perfectly)_
* __-index__ or __-i__ : Use the map at the specified index (if you have 7 map on the result without that filter, -index 4 use the 4th map from theses 7 maps)  
The index filter can be written multiple times to select many maps.
* __-l__ or __-last__ : Use the result of the last !level or !play command
* __-p__ or __-page__ : Show the specified page of level (10 level per page)
* __-all__ : Only for the !play command, if you use this filter, all the found maps are added on the playlist  

#### Examples
__!play -a snowstate -m sprint -all__ : Adds all the maps that snowstate have created on the sprint gamemode to the playlist.  
__!list -name epic level__ : List all the levels on the current gamemode that contains "epic level" on their name. It's equivalent as __!list epic level__  
__!play -name up -mode challenge -index 1__ : Add the first level from challenge mode found that contains "up" in it's name.  
__!level -last -p 4__ : Shows the 4th page (result 30 to 39) of the last command.

# Settings
When the plugin is started for the first time, it generate a setting file that look like this : (on json)
```
{
	"playersCanAddMap" : true,
	"addOneMapOnly" : true,
	"win" : [
		"ALL RIGHT!",
		"YEAH!"
	],
	"rip" : [
		"ACCESS VIOLATION!",
		"AW, THAT'S TOO BAD!",
		"YOU'RE OUT OF CONTROL!"
	],
	"voteNext" : true
}
```
__playersCanAddMap__ (true/false): Allows players to add map on the playlist.  
You can change it with the command !settings play.  
__addOneMapOnly__ (true/false): If players are allowed to add map and this option set to true, they can only add one map at a time.  
You can change it with the command !settings addOne.  
__voteNext__ (true/false): Allows players to vote for the next map on auto mode.  
You can change it with the command !settings vote.  
__win__ (list of string): A list of sentence that will be picked randomly when a player use the command !win  
__rip__ (list of string) : A list of sentence that will be picked randomly when a player use the command !rip  

# Author contacts 
Steam : http://steamcommunity.com/id/larnin/  
Discord : Nico#5480 (https://discord.gg/0SlqqvzfIbi6zhbY)


