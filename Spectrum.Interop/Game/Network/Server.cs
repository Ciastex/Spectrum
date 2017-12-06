using System;
using System.Reflection;
using Spectrum.Interop.Game.EventArgs.Network;
using UnityEngine;

namespace Spectrum.Interop.Game.Network
{
    public class Server
    {
        public static event EventHandler LobbyInitialized;
        public static event EventHandler<PlayerEventArgs> PlayerJoined;
        public static event EventHandler<PlayerEventArgs> PlayerLeft;
        public static event EventHandler<GameModeChangedEventArgs> GameModeChanged;

        static Server()
        {
            Events.GameLobby.Initialized.Subscribe(data =>
            {
                LobbyInitialized?.Invoke(null, System.EventArgs.Empty);
            });

            Events.ServerToClient.AddClient.Subscribe(data =>
            {
                if (G.Sys.NetworkingManager_.IsOnline_)
                {
                    var eventArgs = new PlayerEventArgs(data.clientName_, data.ready_, (LevelCompatibility)data.status_);
                    PlayerJoined?.Invoke(null, eventArgs);
                }
            });

            Events.ServerToClient.RemovePlayerFromClientList.Subscribe(data =>
            {
                var serverLogicObject = GameObject.Find("GameManager")?.GetComponent<ServerLogic>();
                var clientLogic = serverLogicObject?.GetType().GetField("clientLogic_", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(serverLogicObject) as ClientLogic;
                var clientInfo = clientLogic?.GetPlayerInfo(data.playerIndex_);

                if (clientInfo != null)
                {
                    var eventArgs = new PlayerEventArgs(clientInfo.Username_, clientInfo.Ready_, (LevelCompatibility)clientInfo.LevelCompatabilityStatus_);
                    PlayerLeft?.Invoke(null, eventArgs);
                }
            });

            Events.ServerToClient.SetGameMode.Subscribe(data =>
            {
                if (G.Sys.NetworkingManager_.IsOnline_)
                {
                    var eventArgs = new GameModeChangedEventArgs(data.mode_);
                    GameModeChanged?.Invoke(null, eventArgs);
                }
            });
        }
    }
}
