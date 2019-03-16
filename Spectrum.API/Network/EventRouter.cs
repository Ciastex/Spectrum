using Spectrum.API.Interfaces.Systems;
using Spectrum.API.Network.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spectrum.API.Network
{
    internal class EventRouter : IEventRouter
    {
        internal Dictionary<string, List<Action<NetworkPlayer, string>>> ClientToServerCallbacks { get; }
        internal Dictionary<string, List<Action<NetworkPlayer, string>>> ServerToClientCallbacks { get; }
        internal Dictionary<string, List<Action<NetworkPlayer, string>>> BroadcastAllCallbacks { get; }

        internal EventRouter()
        {
            ClientToServerCallbacks = new Dictionary<string, List<Action<NetworkPlayer, string>>>();
            ServerToClientCallbacks = new Dictionary<string, List<Action<NetworkPlayer, string>>>();
            BroadcastAllCallbacks = new Dictionary<string, List<Action<NetworkPlayer, string>>>();

            ClientToServer.Subscribe(ClientToServerEventReceived);
            ServerToClient.Subscribe(ServerToClientEventReceived);
            BroadcastAll.Subscribe(BroadcastAllEventReceived);
        }

        public void RegisterClientToServerCallback(string eventName, Action<NetworkPlayer, string> callback)
        {
            if (!ClientToServerCallbacks.ContainsKey(eventName))
                ClientToServerCallbacks.Add(eventName, new List<Action<NetworkPlayer, string>>());

            ClientToServerCallbacks[eventName].Add(callback);
        }

        public void RegisterServerToClientCallback(string eventName, Action<NetworkPlayer, string> callback)
        {
            if (!ServerToClientCallbacks.ContainsKey(eventName))
                ServerToClientCallbacks.Add(eventName, new List<Action<NetworkPlayer, string>>());

            ServerToClientCallbacks[eventName].Add(callback);
        }

        public void RegisterBroadcastAllCallback(string eventName, Action<NetworkPlayer, string> callback)
        {
            if (!BroadcastAllCallbacks.ContainsKey(eventName))
                BroadcastAllCallbacks.Add(eventName, new List<Action<NetworkPlayer, string>>());

            BroadcastAllCallbacks[eventName].Add(callback);
        }

        private void ClientToServerEventReceived(ClientToServer.Data data)
        {
            if (ClientToServerCallbacks.ContainsKey(data.EventName))
                ClientToServerCallbacks[data.EventName].ForEach((action) => action?.Invoke(data.Sender, data.EventData));
        }

        private void ServerToClientEventReceived(ServerToClient.Data data)
        {
            if (ServerToClientCallbacks.ContainsKey(data.EventName))
                ServerToClientCallbacks[data.EventName].ForEach((action) => action?.Invoke(data.Sender, data.EventData));
        }

        private void BroadcastAllEventReceived(BroadcastAll.Data data)
        {
            if (BroadcastAllCallbacks.ContainsKey(data.EventName))
                BroadcastAllCallbacks[data.EventName].ForEach((action) => action?.Invoke(data.Sender, data.EventData));
        }
    }
}
