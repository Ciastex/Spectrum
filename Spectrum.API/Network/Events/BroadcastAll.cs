using Events;
using UnityEngine;

namespace Spectrum.API.Network.Events
{
    public class BroadcastAll : BroadcastAllEvent<BroadcastAll.Data>
    {
        public struct Data : IBitSerializable, INetworkGrouped
        {
            public string EventName;
            public string EventData;
            public NetworkPlayer Sender;

            public NetworkGroup NetworkGroup_ { get; }

            public Data(string eventName, string eventData)
            {
                EventName = eventName;
                EventData = eventData;
                Sender = UnityEngine.Network.player;

                NetworkGroup_ = NetworkGroup.GlobalGroup;
            }

            void IBitSerializable.Serialize(BitStreamAbstract stream)
            {
                stream.Serialize(ref EventName);
                stream.Serialize(ref EventData);
                stream.Serialize(ref Sender);
            }
        }
    }
}
