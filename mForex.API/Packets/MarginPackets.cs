using ProtoBuf;

namespace mForex.API.Packets
{
    [ProtoContract]
    public class MarginLevelPacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public MarginLevel MarginLevel { get; set; }

        public MarginLevelPacket()
            : base(APINetworkPacketType.MarginLevel)
        {
        }
    }

    [ProtoContract]
    public class MarginLevelRequestPacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        public MarginLevelRequestPacket()
            : base(APINetworkPacketType.MarginLevelRequest)
        { }

        public MarginLevelRequestPacket(int requestId)
            : base(APINetworkPacketType.MarginLevelRequest)
        {
            RequestId = requestId;
        }
    }

    [ProtoContract]
    public class MarginLevelResponsePacket : APINetworkPacket, IIdentifiable
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public MarginLevel MarginLevel { get; set; }

        public MarginLevelResponsePacket()
            : base(APINetworkPacketType.MarginLevelResponse)
        { }
    }

    [ProtoContract]
    public class MarginLevel
    {
        [ProtoMember(1, IsRequired = true)]
        public int Login { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public double Balance { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public double Equity { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public double FreeMargin { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public Meta4MarginLevelType LevelType { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public double Margin { get; set; }

        [ProtoMember(7, IsRequired = true)]
        public double Level { get; set; }
    }

    public enum Meta4MarginLevelType
    {
        Ok = 0,
        MarginCall = 1,
        StopOut = 2,
    }
}
