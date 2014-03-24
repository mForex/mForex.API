using ProtoBuf;

namespace mForex.API.Packets
{
    [ProtoContract]
    public class HeartBeatRequestPacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        public HeartBeatRequestPacket()
            : base(APINetworkPacketType.HeartBeatRequest)
        { }

        public HeartBeatRequestPacket(int requestId)
            : base(APINetworkPacketType.HeartBeatRequest)
        {
            RequestId = requestId;
        }
    }

    [ProtoContract]
    public class HeartBeatResponsePacket : APINetworkPacket, IIdentifiable
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        public HeartBeatResponsePacket()
            : base(APINetworkPacketType.HeartBeatResponse)
        { }

        public HeartBeatResponsePacket(int requestId)
            : base(APINetworkPacketType.HeartBeatResponse)
        {
            RequestId = requestId;
        }
    }
}
