using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using mForex.API.Data;

namespace mForex.API.Packets
{
    [ProtoContract]
    public class CandleRequestPacket : APINetworkPacket
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public DateTime FromTime { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public DateTime ToTime { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public string Symbol { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public CandlePeriod Period { get; set; }

        public CandleRequestPacket()
            : base(APINetworkPacketType.CandleRequest)
        { }

        public CandleRequestPacket(int requestId, string symbol, CandlePeriod period, DateTime from, DateTime to)
            : base(APINetworkPacketType.CandleRequest)
        {
            RequestId = requestId;
            Symbol = symbol;
            Period = period;
            FromTime = from;
            ToTime = to;
        }
    }

    [ProtoContract]
    public class CandleResponsePacket : APINetworkPacket,  IIdentifiable
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public DateTime FromTime { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public DateTime ToTime { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public string Symbol { get; set; }

        [ProtoMember(7, IsRequired = true)]
        public Candle[] Candles { get; set; }

        [ProtoMember(8, IsRequired = true)]
        public CandlePeriod Period { get; set; }

        public CandleResponsePacket()
            : base(APINetworkPacketType.CandleResponse)
        {
        }
    }
}
