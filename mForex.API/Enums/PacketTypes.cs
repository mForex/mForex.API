using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mForex.API
{
    public enum APINetworkPacketType
    {
        LoginRequest = 1,
        LogoutRequest = 2,
        CandleRequest = 4,
        InstrumentSettingsRequest = 5,
        MarginLevelRequest = 6,
        ClosedTradesRequest = 7,
        TradesInfoRequest = 8,
        TradeTransRequest = 9,
        SessionScheduleRequest = 10,
        HeartBeatRequest = 11,

        LoginResponse = 257,
        CandleResponse = 260,
        InstrumentSettingsResponse = 261,
        MarginLevelResponse = 262,
        ClosedTradesResponse = 263,
        TradesInfoResponse = 264,
        TradeTransResponse = 265,
        SessionScheduleResponse = 266,
        HeartBeatResponse = 267,

        Ticks = 513,
        MarginLevel = 514,
        TradeUpdate = 515,
    }
    
    [ProtoContract]
    public enum APIErrorCode
    {
        [ProtoEnum]
        OK = 0,
        [ProtoEnum]
        ServerError = 1,
        [ProtoEnum]
        UndefinedError = 2,
        [ProtoEnum]
        TradeError = 3,
    }

    public abstract class APINetworkPacket
    {
        public APINetworkPacketType Type { get; private set; }

        public APINetworkPacket(APINetworkPacketType type)
        {
            Type = type;
        }
    }
}
