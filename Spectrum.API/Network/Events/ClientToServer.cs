using Events;
using System.Text;
using UnityEngine;

namespace Spectrum.API.Network.Events
{
    public class ClientToServer : ClientToServerEvent<ClientToServer.Data>
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

            public Data(string eventName, string eventData, NetworkGroup networkGroup = NetworkGroup.GlobalGroup)
                : this(eventName, Encoding.UTF8.GetBytes(eventData), networkGroup) { }

            void IBitSerializable.Serialize(BitStreamAbstract stream)
            {
                stream.Serialize(ref EventName);
                stream.Serialize(EventData, 0, EventData.Length);

                stream.Serialize(ref Sender);
            }
        }
    }
}
