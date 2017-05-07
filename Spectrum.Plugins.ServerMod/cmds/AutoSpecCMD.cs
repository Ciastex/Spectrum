using Events;
using Events.GameMode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class AutoSpecCMD : cmd
    {
        private bool autoSpecMode = false;

        public override string name { get { return "autospec"; } }
        public override PermType perm { get { return PermType.LOCAL; } }
        public override bool canUseAsClient { get { return true; } }

        public AutoSpecCMD()
        {
            Events.GameMode.Go.Subscribe(data =>
            {
                onModeStart();
            });
        }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!autospec: Toggle automatic spectating for you.");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            if(autoSpecMode)
            {
                autoSpecMode = false;
                Utilities.sendMessage("Auto spectator mode turned off");
                return;
            }

            autoSpecMode = true;
            Utilities.sendMessage("Auto spectator mode turned on");
            onModeStart();
        }

        private void onModeStart()
        {
            if (!Utilities.isOnline())
                autoSpecMode = false;
            if (autoSpecMode)
                G.Sys.GameManager_.StartCoroutine(spectate());
        }

        IEnumerator spectate()
        {
            yield return new WaitForSeconds(1.0f);
            var players = G.Sys.PlayerManager_.PlayerList_;
            if (players.Count != 0)
            {
                if (players.Count == 1 && Utilities.isHost())
                    G.Sys.GameManager_.GoToLobby();
                else
                {
                    var localPlayer = G.Sys.PlayerManager_.Current_.playerData_;
                    localPlayer.Spectate();
                    /*var p = players[0];
                    if (p.IsLocal_)
                        StaticTargetedEvent<Finished.Data>.Broadcast(p.NetworkPlayer_, default(Finished.Data));*/
                }
            }
            yield return null;
        }
    }
}
