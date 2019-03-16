using Spectrum.API.Extensions;
using Spectrum.API.Reflection;
using UnityEngine;

namespace Spectrum.API.Network
{
    public static class NetworkOverrides
    {
        internal static void RegisterServerToClientEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var serverToClientTranscieverInstance = GameObject.FindObjectOfType<ServerToClientNetworkTransceiver>();

            (serverToClientTranscieverInstance as NetworkStaticEventTransceiver)
                .CallPrivateGenericMethod<T>(new MethodMetadata
                {
                    IsStatic = false,
                    Name = "RegisterServerToClientEvent"
                });
        }

        internal static void RegisterTargetedEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var serverToClientTranscieverInstance = GameObject.FindObjectOfType<ServerToClientNetworkTransceiver>();

            (serverToClientTranscieverInstance as NetworkStaticEventTransceiver)
                .CallPrivateGenericMethod<T>(new MethodMetadata
                {
                    IsStatic = false,
                    Name = "RegisterTargetedEvent"
                });
        }

        internal static void RegisterClientToServerEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var clientToServerTranscieverInstance = GameObject.FindObjectOfType<ClientToServerNetworkTransceiver>();

            (clientToServerTranscieverInstance as NetworkStaticEventTransceiver)
                .CallPrivateGenericMethod<T>(new MethodMetadata
                {
                    IsStatic = false,
                    Name = "RegisterClientToServerEvent"
                });
        }

        internal static void RegisterBroadcastAllEvent<T>() where T : struct, IBitSerializable, INetworkGrouped
        {
            var clientToServerTranscieverInstance = GameObject.FindObjectOfType<ClientToClientNetworkTransceiver>();

            (clientToServerTranscieverInstance as NetworkStaticEventTransceiver)
                .CallPrivateGenericMethod<T>(new MethodMetadata
                {
                    IsStatic = false,
                    Name = "RegisterBroadcastAllEvent"
                });
        }
    }
}
