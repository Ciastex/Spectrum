using Events;
using UnityEngine;

namespace Spectrum.API.Network.Events
{
    public class BroadcastAll : BroadcastAllEvent<BroadcastAll.Data>
    {
        public struct Data : IBitSerializable, INetworkGrouped
        {
            public string EventName;
            public byte[] EventData;

            public NetworkPlayer Sender;
            public NetworkGroup NetworkGroup_ { get; }

            public Data(string eventName, byte[] eventData)
            {
                EventName = eventName;
                EventData = eventData;

                Sender = UnityEngine.Network.player;
                NetworkGroup_ = NetworkGroup.GlobalGroup;
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
