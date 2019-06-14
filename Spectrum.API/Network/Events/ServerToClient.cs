using Events;
using UnityEngine;

namespace Spectrum.API.Network.Events
{
    public class ServerToClient : ServerToClientEvent<ServerToClient.Data>
    {
        public struct Data : IBitSerializable, INetworkGrouped
        {
            public string EventName;
            public byte[] EventData;

            public NetworkPlayer Sender;
            public NetworkGroup NetworkGroup_ { get; }

            public Data(string eventName, byte[] eventData, NetworkGroup networkGroup = NetworkGroup.GlobalGroup)
            {
                EventName = eventName;
                EventData = eventData;

                Sender = UnityEngine.Network.player;
                NetworkGroup_ = networkGroup;
            }

            void IBitSerializable.Serialize(BitStreamAbstract stream)
            {
                stream.Serialize(ref EventName);
                stream.Serialize(EventData, 0, EventData.Length);

                stream.Serialize(ref Sender);
            }
        }
    }
}
