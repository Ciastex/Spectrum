using UnityEngine;

namespace Spectrum.API.Network
{
    internal static class NetworkOverrides
    {
        internal static void RegisterServerToClientEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var serverToClientTranscieverInstance = GameObject.FindObjectOfType<ServerToClientNetworkTransceiver>();
            (serverToClientTranscieverInstance as NetworkStaticEventTransceiver).RegisterServerToClientEvent<T>();
        }

        internal static void RegisterTargetedEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var serverToClientTranscieverInstance = GameObject.FindObjectOfType<ServerToClientNetworkTransceiver>();
            (serverToClientTranscieverInstance as NetworkStaticEventTransceiver).RegisterTargetedEvent<T>();
        }

        internal static void RegisterClientToServerEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var clientToServerTranscieverInstance = GameObject.FindObjectOfType<ClientToServerNetworkTransceiver>();
            (clientToServerTranscieverInstance as NetworkStaticEventTransceiver).RegisterClientToServerEvent<T>();
        }

        internal static void RegisterBroadcastAllEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var clientToServerTranscieverInstance = GameObject.FindObjectOfType<ClientToClientNetworkTransceiver>();
            (clientToServerTranscieverInstance as NetworkStaticEventTransceiver).RegisterBroadcastAllEvent<T>();
        }
    }
}
