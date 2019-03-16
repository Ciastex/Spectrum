using System;
using UnityEngine;

namespace Spectrum.API.Interfaces.Systems
{
    public interface IEventRouter
    {
        void RegisterClientToServerCallback(string eventName, Action<NetworkPlayer, string> callback);
        void RegisterServerToClientCallback(string eventName, Action<NetworkPlayer, string> callback);
        void RegisterBroadcastAllCallback(string eventName, Action<NetworkPlayer, string> callback);
    }
}
