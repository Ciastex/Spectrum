using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class LoadCMD : cmd
    {
        public override string name { get { return "load"; } }
        public override PermType perm { get { return PermType.HOST; } }
        public override bool canUseAsClient { get { return false; } }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!load: Show all the available playlists.");
            Utilities.sendMessage("!load [playlist name]: Load a playlist.");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            if (message == "")
            {
                printPlaylists();
                return;
            }

            var name = Resource.PersonalLevelPlaylistsDirPath_  + message + ".xml";
            if(!Resource.FileExist(name))
            {
                Utilities.sendMessage("The playlist " + message + " don't exist !");
                return;
            }
            
            var item = G.Sys.GameManager_.LevelPlaylist_.Count_ == 0 ? null : G.Sys.GameManager_.LevelPlaylist_.Playlist_[G.Sys.GameManager_.LevelPlaylist_.Index_];

            var gameObject = LevelPlaylist.Load(name);
            var playlistComp = gameObject.GetComponent<LevelPlaylist>();
            G.Sys.GameManager_.LevelPlaylist_.Copy(playlistComp);

            Utilities.sendMessage("Playlist Loaded : " + G.Sys.GameManager_.LevelPlaylist_.Count_ + " levels.");

            if (Utilities.isOnGamemode() && item != null)
                G.Sys.GameManager_.LevelPlaylist_.Playlist_.Insert(0, item);
        }

        void printPlaylists()
        {
            var liste = Utilities.playlists();
            liste.RemoveAll((string s) => !Resource.FileExist(s));
            Utilities.sendMessage(liste.Count + " playlists found :");
            foreach (var v in liste)
                Utilities.sendMessage(Resource.GetFileNameWithoutExtension(v));
        }
    }
}
