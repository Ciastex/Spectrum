using Spectrum.API;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using Spectrum.API.Game.Network;
using System.Collections.Generic;
using Spectrum.Plugins.ServerMod.cmds;
using System;
using Spectrum.API.Configuration;
using System.Linq;

namespace Spectrum.Plugins.ServerMod
{
    public class Entry : IPlugin
    {
        public string FriendlyName => "Server commands Mod";
        public string Author => "Nico";
        public string Contact => "SteamID: larnin";
        public APILevel CompatibleAPILevel => APILevel.UltraViolet;
        public string PluginVersion = "V0.4";

        private static Settings Settings = new Settings(typeof(Entry));

        public void Initialize(IManager manager)
        {
            load();

            if (G.Sys.GameManager_.ModeID_ == GameModeID.Trackmogrify)
            {
                Utilities.sendMessage("You can't load a playlist in trackmogrify");
                return;
            }

            Events.Local.ChatSubmitMessage.Subscribe(data =>
            {
                Chat_MessageSent(data.message_);
            });

            Events.ClientToAllClients.ChatMessage.Subscribe(data =>
            {
                var author = Utilities.ExtractMessageAuthor(data.message_);
                var steamName = G.Sys.SteamworksManager_.GetUserName().ToLower().Trim();
                var profileName = G.Sys.PlayerManager_.Current_.profile_.Name_.ToLower().Trim();

                if (!Utilities.IsSystemMessage(data.message_) && (author.ToLower().Trim() != steamName && author.ToLower().Trim() != profileName))
                    Chat_MessageReceived(author, Utilities.ExtractMessageBody(data.message_));
            });
        }

        private void Chat_MessageSent(string message)
        {
            var client = Utilities.localClient();
            if (client == null)
                Console.WriteLine("Error: Local client can't be found !");

            if (message.StartsWith("%"))
            {
                if (Utilities.isHost())
                    return;

                int pos = message.IndexOf(' ');
                string commandName = (pos > 0 ? message.Substring(1, pos) : message.Substring(1).Trim());
                cmd c = cmd.all.getCommand(commandName);
                if (c == null)
                    return;
                if (!c.canUseAsClient && c.perm != PermType.LOCAL)
                {
                    Utilities.sendMessage("You can't use that command as client");
                    return;
                }

                exec(c, client, pos > 0 ? message.Substring(pos + 1).Trim() : "");
            }
            else
            {
                if (!message.StartsWith("!"))
                    return;

                if (message.ToLower().Trim() == "!plugin")
                {
                    printClient();
                    return;
                }

                if (!Utilities.isHost())
                    return;

                int pos = message.IndexOf(' ');
                string commandName = (pos > 0 ? message.Substring(1, pos) : message.Substring(1)).Trim();
                cmd c = cmd.all.getCommand(commandName);
                if (c == null)
                {
                    Utilities.sendMessage("The command '" + commandName + "' don't exist.");
                    return;
                }

                exec(c, client, pos > 0 ? message.Substring(pos + 1).Trim() : "");
            }
        }

        private void Chat_MessageReceived(string author, string message)
        {
            if (!message.StartsWith("!"))
                return;

            if (message.ToLower().Trim() == "!plugin")
            {
                printClient();
                return;
            }

            if (!Utilities.isHost())
                return;

            var client = Utilities.clientFromName(author);
            if (client == null)
            {
                Console.WriteLine("Error: client can't be found");
                return;
            }
                
            int pos = message.IndexOf(' ');
            string commandName = (pos > 0 ? message.Substring(1, pos) : message.Substring(1)).Trim();
            cmd c = cmd.all.getCommand(commandName);

            if (c == null)
            {
                Utilities.sendMessage("The command '" + commandName + "' don't exist.");
                return;
            }

            if (c.perm == PermType.LOCAL)
                return;

            if(c.perm != PermType.ALL)
            {
                Utilities.sendMessage("You don't have the permission to do that !");
                return;
            }

            exec(c, client, pos > 0 ? message.Substring(pos + 1).Trim() : "");
        }

        private void exec(cmd c, ClientPlayerInfo p, string message)
        {
            try
            {
                c.use(p, message);
            }
            catch (Exception error)
            {
                Utilities.sendMessage("Error");
                Console.WriteLine(error);
            }
        }

        public void Shutdown()
        {
            
        }

        private void printClient()
        {
            Utilities.sendMessage(Utilities.localClient().GetChatName() + " " + PluginVersion);
        }

        public static void load()
        {
            ValidateSettings();

            try
            {
                PlayCMD.playersCanAddMap = (bool)Settings["playersCanAddMap"];
                PlayCMD.addOneMapOnly = (bool)Settings["addOneMapOnly"];
                AutoCMD.voteNext = (bool)Settings["voteNext"];
                WinCMD.winList = ((string[])Settings["win"]).ToList();
                RipCMD.ripList = ((string[])Settings["rip"]).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                Console.WriteLine(e.Source);
            }
        }

        public static void save()
        {
            Settings["playersCanAddMap"] = PlayCMD.playersCanAddMap;
            Settings["addOneMapOnly"] = PlayCMD.addOneMapOnly;
            Settings["voteNext"] = AutoCMD.voteNext;

            Settings.Save();
        }

        private static void ValidateSettings()
        {
            if (!Settings.ContainsKey("playersCanAddMap"))
                Settings["playersCanAddMap"] = false;
            if (!Settings.ContainsKey("addOneMapOnly"))
                Settings["addOneMapOnly"] = true;
            if (!Settings.ContainsKey("voteNext"))
                Settings["voteNext"] = false;
            if (!Settings.ContainsKey("win"))
                Settings["win"] = WinCMD.winList;
            if (!Settings.ContainsKey("rip"))
                Settings["rip"] = RipCMD.ripList;

            Settings.Save();
        }
    }
}
