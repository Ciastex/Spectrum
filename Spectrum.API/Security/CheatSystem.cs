using Events;
using Events.Network;
using Events.Scene;
using Newtonsoft.Json;
using Spectrum.API.Events.EventArgs;
using Spectrum.API.Extensions;
using Spectrum.API.Interfaces.Systems;
using Spectrum.API.Network;
using Spectrum.API.Network.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Spectrum.API.Security
{
    internal class CheatSystem : ICheatSystem
    {
        private readonly Dictionary<Assembly, List<string>> _cheatStates;
        private readonly SubscriberList _subscriberList;

        private Logging.Logger Logger { get; }
        private IManager Manager { get; }

        public bool AnyCheatsEnabled => _cheatStates.Values.Any(x => x.Any());

        public event EventHandler<CheatStateInfoEventArgs> CheatStateInfoReceived;
        public event EventHandler<CheatStateFailureEventArgs> CheatStateInfoFailure;

        internal CheatSystem(IManager manager)
        {
            _cheatStates = new Dictionary<Assembly, List<string>>();

            _subscriberList = new SubscriberList
            {
                new StaticEvent<LoadFinish.Data>.Subscriber(OnSceneLoadFinished),
                new StaticEvent<ClientConnected.Data>.Subscriber(OnClientConnected)
            };
            _subscriberList.Subscribe();

            Logger = new Logging.Logger("cheatsystem.log");

            Manager = manager;
            manager.EventRouter.RegisterServerToClientCallback(EventNames.CheatStateInfoRequest, OnCheatStateInfoRequested);
            manager.EventRouter.RegisterClientToServerCallback(EventNames.CheatStateInfoResponse, OnCheatStateInfoReceived);
        }

        public void Enable(string key)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            if (!_cheatStates.ContainsKey(callingAssembly))
                _cheatStates.Add(callingAssembly, new List<string>() { key });
            else
            {
                if (!_cheatStates[callingAssembly].Contains(key))
                    _cheatStates[callingAssembly].Add(key);
                else Logger.Warning($"Plugin assembly {Path.GetFileName(callingAssembly.Location)} tried to add cheat key '{key}' more than once.");
            }
        }

        public void Disable(string key)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            if (_cheatStates.ContainsKey(callingAssembly))
            {
                if (_cheatStates[callingAssembly].Contains(key))
                    _cheatStates[callingAssembly].Remove(key);
                else Logger.Warning($"Plugin assembly {Path.GetFileName(callingAssembly.Location)} tried to remove non-existent cheat key '{key}'.");
            }
        }

        private void OnSceneLoadFinished(LoadFinish.Data data)
        {
            G.Sys.CheatsManager_.anyGameplayCheatsUsedThisLevel_ = AnyCheatsEnabled;
        }

        private void OnClientConnected(ClientConnected.Data data)
        {
            StaticTargetedEvent<ServerToClient.Data>.Broadcast(
                data.player_,
                new ServerToClient.Data(
                    EventNames.CheatStateInfoRequest,
                    string.Empty
                )
            );
        }

        private void OnCheatStateInfoRequested(NetworkPlayer sender, string json)
        {
#pragma warning disable 0618
            StaticTransceivedEvent<ClientToServer.Data>.Broadcast(
#pragma warning restore 0618
                new ClientToServer.Data(
                    EventNames.CheatStateInfoResponse,
                    JsonConvert.SerializeObject(new CheatStateInfo(AnyCheatsEnabled))
                )
            );
        }

        private void OnCheatStateInfoReceived(NetworkPlayer sender, string json)
        {
            if (sender == UnityEngine.Network.player) return;

            try
            {
                var info = JsonConvert.DeserializeObject<CheatStateInfo>(json);
                CheatStateInfoReceived?.Invoke(this, new CheatStateInfoEventArgs(info, sender));
            }
            catch (Exception e)
            {
                CheatStateInfoFailure?.Invoke(this, new CheatStateFailureEventArgs(sender, e, json));
            }
        }
    }
}
