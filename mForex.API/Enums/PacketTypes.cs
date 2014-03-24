
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
        AccountSettingsRequest = 12,

        LoginResponse = 257,
        CandleResponse = 260,
        InstrumentSettingsResponse = 261,
        MarginLevelResponse = 262,
        ClosedTradesResponse = 263,
        TradesInfoResponse = 264,
        TradeTransResponse = 265,
        SessionScheduleResponse = 266,
        HeartBeatResponse = 267,
        AccountSettingsResponse = 268,

        Ticks = 513,
        MarginLevel = 514,
        TradeUpdate = 515,
    }    
}
