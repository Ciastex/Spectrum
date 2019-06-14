using Spectrum.API.Interfaces.Systems;
using Spectrum.API.Network.Events;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Logger = Spectrum.API.Logging.Logger;

namespace Spectrum.API.Network
{
    internal class EventRouter : IEventRouter
    {
        private IManager Manager { get; }
        private Logger Logger { get; }

        internal Dictionary<string, List<Action<NetworkPlayer, string>>> ClientToServerCallbacks { get; }
        internal Dictionary<string, List<Action<NetworkPlayer, string>>> ServerToClientCallbacks { get; }
        internal Dictionary<string, List<Action<NetworkPlayer, string>>> BroadcastAllCallbacks { get; }

        internal EventRouter(IManager manager)
        {
            Manager = manager;
            Logger = new Logger(Defaults.EventRouterLogFileName);

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
            if (Manager.GetConfig<bool>("NetworkDebugging"))
            {
                var sb = new StringBuilder();
                sb.AppendLine("\nR: [CLIENT -> SERVER]");
                sb.AppendLine($"  NETWORK GROUP: {data.NetworkGroup_.ToString()}");
                sb.AppendLine($"  SENDER");
                sb.AppendLine($"    INTERNAL {data.Sender.ipAddress}:{data.Sender.port}");
                sb.AppendLine($"    EXTERNAL {data.Sender.externalIP}:{data.Sender.externalPort}");
                sb.AppendLine($"    GUID {data.Sender.guid}");

                sb.AppendLine($"  EVENT NAME: {data.EventName}");
                sb.AppendLine($"  EVENT DATA: {data.EventData}");

                Logger.Info(sb.ToString());
            }

            if (ClientToServerCallbacks.ContainsKey(data.EventName))
                ClientToServerCallbacks[data.EventName].ForEach((action) => action?.Invoke(data.Sender, ToHexDump(data.EventData)));
        }

        private void ServerToClientEventReceived(ServerToClient.Data data)
        {
            if (Manager.GetConfig<bool>("NetworkDebugging"))
            {
                var sb = new StringBuilder();
                sb.AppendLine("\nR: [SERVER -> CLIENT]");
                sb.AppendLine($"  NETWORK GROUP: {data.NetworkGroup_.ToString()}");
                sb.AppendLine($"  SENDER");
                sb.AppendLine($"    INTERNAL {data.Sender.ipAddress}:{data.Sender.port}");
                sb.AppendLine($"    EXTERNAL {data.Sender.externalIP}:{data.Sender.externalPort}");
                sb.AppendLine($"    GUID {data.Sender.guid}");

                sb.AppendLine($"  EVENT NAME: {data.EventName}");
                sb.AppendLine($"  EVENT DATA: {data.EventData}");

                Logger.Info(sb.ToString());
            }

            if (ServerToClientCallbacks.ContainsKey(data.EventName))
                ServerToClientCallbacks[data.EventName].ForEach((action) => action?.Invoke(data.Sender, ToHexDump(data.EventData)));
        }

        private void BroadcastAllEventReceived(BroadcastAll.Data data)
        {
            if (Manager.GetConfig<bool>("NetworkDebugging"))
            {
                var sb = new StringBuilder();
                sb.AppendLine("\nR: [BROADCAST - ALL]");
                sb.AppendLine($"  NETWORK GROUP: {data.NetworkGroup_.ToString()}");
                sb.AppendLine($"  SENDER");
                sb.AppendLine($"    INTERNAL {data.Sender.ipAddress}:{data.Sender.port}");
                sb.AppendLine($"    EXTERNAL {data.Sender.externalIP}:{data.Sender.externalPort}");
                sb.AppendLine($"    GUID {data.Sender.guid}");

                sb.AppendLine($"  EVENT NAME: {data.EventName}");
                sb.AppendLine($"  EVENT DATA: {data.EventData}");

                Logger.Info(sb.ToString());
            }

            if (BroadcastAllCallbacks.ContainsKey(data.EventName))
                BroadcastAllCallbacks[data.EventName].ForEach((action) => action?.Invoke(data.Sender, ToHexDump(data.EventData)));
        }

        private string ToHexDump(byte[] data)
        {
            var sb = new StringBuilder();

            foreach (var b in data)
                sb.Append($"{b:X2}");

            return sb.ToString();
        }
    }
}
