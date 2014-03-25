using ProtoBuf;

namespace mForex.API.Packets
{
    [ProtoContract]
    public class AccountSettingsRequestPacket : APINetworkPacket
    {
    
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        public AccountSettingsRequestPacket()
            : base(APINetworkPacketType.AccountSettingsRequest)
        {    
        }

        public AccountSettingsRequestPacket(int requestId)
            : base(APINetworkPacketType.AccountSettingsRequest)
        {
            RequestId = requestId;
        }
    }

    [ProtoContract]
    public class AccountSettingsResponsePacket : APINetworkPacket, IIdentifiable
    {
    
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; private set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; private set; }

        [ProtoMember(4, IsRequired = true)]
        public AccountSettings AccountSettings { get; private set; }

        public AccountSettingsResponsePacket()
            : base(APINetworkPacketType.AccountSettingsResponse)
        {
        }

        public AccountSettingsResponsePacket(int requestId, APIErrorCode error)
            : base(APINetworkPacketType.AccountSettingsResponse)
        {
            this.RequestId = requestId;
            this.Status = false;
            this.ErrorCode = error;
        }

        public AccountSettingsResponsePacket(int requestId, AccountSettings settings)
            : base(APINetworkPacketType.AccountSettingsResponse)
        {
            this.RequestId = requestId;
            this.Status = true;
            this.ErrorCode = APIErrorCode.OK;

            this.AccountSettings = settings;
        }
    }

    [ProtoContract]
    public class AccountSettings
    {
        [ProtoMember(1, IsRequired = true)]
        public string Name { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public int Leverage { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public double InterestRate { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public double MarginCall { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public double MarginStopOut { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public MarginMode MarginMode { get; set; }

        [ProtoMember(7, IsRequired = true)]
        public MarginType MarginType { get; set; }

        [ProtoMember(8, IsRequired = true)]
        public AccountType AccountType { get; set; }
    }
}
