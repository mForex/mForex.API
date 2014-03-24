using System;
using ProtoBuf;

namespace mForex.API.Packets
{
    [ProtoContract]
    public class SessionScheduleRequestPacket : APINetworkPacket
    {

        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public string Symbol { get; set; }

        public SessionScheduleRequestPacket()
            : base(APINetworkPacketType.SessionScheduleRequest)
        {
        }

        public SessionScheduleRequestPacket(int requestId, string symbol)
            : base(APINetworkPacketType.SessionScheduleRequest)
        {
            this.RequestId = requestId;
            this.Symbol = symbol;
        }
    }

    [ProtoContract]
    public class SessionScheduleResponsePacket : APINetworkPacket, IIdentifiable
    {
        [ProtoMember(1, IsRequired = true)]
        public int RequestId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public bool Status { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public APIErrorCode ErrorCode { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public string Symbol { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public DailySession[] Sessions { get; set; }

        public SessionScheduleResponsePacket()
            : base(APINetworkPacketType.SessionScheduleResponse)
        {
        }

        public SessionScheduleResponsePacket(int requestId)
            : base(APINetworkPacketType.SessionScheduleResponse)
        {
            this.RequestId = requestId;
            this.ErrorCode = APIErrorCode.OK;
        }

        public SessionScheduleResponsePacket(int requestId, APIErrorCode error)
            : base(APINetworkPacketType.SessionScheduleResponse)
        {
            this.RequestId = requestId;
            this.ErrorCode = error;
        }
    }

    [ProtoContract]
    public class DailySession
    {
        [ProtoMember(1, IsRequired = true)]
        public DayOfWeek DayOfWeek { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public TradingSession[] TradingSessions { get; set; }
    }

    [ProtoContract]
    public class TradingSession
    {
        [ProtoMember(1, IsRequired = true)]
        public TimeSpan OpenTime { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public TimeSpan CloseTime { get; set; }

    }
}

