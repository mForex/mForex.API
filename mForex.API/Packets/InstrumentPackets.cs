using System;
using ProtoBuf;

namespace mForex.API.Packets
{

    [ProtoContract]
    public class InstrumentSettingsRequestPacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        public InstrumentSettingsRequestPacket()
            : base(APINetworkPacketType.InstrumentSettingsRequest)
        { }

        public InstrumentSettingsRequestPacket(int requestId)
            : base(APINetworkPacketType.InstrumentSettingsRequest)
        {
            RequestId = requestId;
        }
    }

    [ProtoContract]
    public class InstrumentSettingsResponsePacket : APINetworkPacket, IIdentifiable
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; private set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; private set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; private set; }

        [ProtoMember(4, IsRequired = true)]
        public InstrumentSettings[] InstrumentSettings { get; private set; }

        public InstrumentSettingsResponsePacket()
            : base(APINetworkPacketType.InstrumentSettingsResponse)
        {
        }
    }

    #region Instruemnt Settings

    
    [ProtoContract]
    public class InstrumentSettings
    {
        public InstrumentSettings()
        { }

        [ProtoMember(1, IsRequired = true)]
        public string Name { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public int Digits { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public double ContractSize { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public ProfitCalcMode ProfitCalculationMode { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public MarginCalcMode MarginCalculationMode { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public double MarginHedged { get; set; }

        [ProtoMember(7, IsRequired = true)]
        public double MarginDivider { get; set; }

        [ProtoMember(8, IsRequired = true)]
        public SwapType SwapType { get; set; }

        [ProtoMember(9, IsRequired = true)]
        public double SwapLong { get; set; }

        [ProtoMember(10, IsRequired = true)]
        public double SwapShort { get; set; }

        [ProtoMember(11, IsRequired = true)]
        public TradeMode TradeMode { get; set; }

        [ProtoMember(12, IsRequired = true)]
        public string Currency { get; set; }

        [ProtoMember(13, IsRequired = true)]
        public double Bid { get; set; }

        [ProtoMember(14, IsRequired = true)]
        public double Ask { get; set; }

        [ProtoMember(15, IsRequired = true)]
        public double Low { get; set; }

        [ProtoMember(16, IsRequired = true)]
        public double High { get; set; }

        [ProtoMember(17, IsRequired = true)]
        public DateTime Time { get; set; }
    }
    #endregion Instrument Settings
}
