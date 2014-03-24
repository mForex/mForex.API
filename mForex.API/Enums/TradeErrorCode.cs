using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum TradeErrorCode
    {
        [ProtoEnum]
        OK = 0,
        [ProtoEnum]
        OKNone = 1,
        [ProtoEnum]
        Error = 2,
        [ProtoEnum]
        InvalidData = 3,
        [ProtoEnum]
        TechnicalProblem = 4,
        [ProtoEnum]
        OldVersion = 5,
        [ProtoEnum]
        NoConnection = 6,
        [ProtoEnum]
        NotEnoughRights = 7,
        [ProtoEnum]
        TooFrequent = 8,
        [ProtoEnum]
        Malfunction = 9,
        [ProtoEnum]
        SecuritySession = 10,
        [ProtoEnum]
        AccountDisabled = 64,
        [ProtoEnum]
        BadAccountInfo = 65,
        [ProtoEnum]
        TradeTimeout = 128,
        [ProtoEnum]
        TradeBadPrices = 129,
        [ProtoEnum]
        TradeBadStops = 130,
        [ProtoEnum]
        TradeBadVolume = 131,
        [ProtoEnum]
        TradeMarketClose = 132,
        [ProtoEnum]
        TradeDisable = 133,
        [ProtoEnum]
        TradeNoMoney = 134,
        [ProtoEnum]
        TradePriceChanged = 135,
        [ProtoEnum]
        TradeOffquotes = 136,
        [ProtoEnum]
        TradeBrokerBusy = 137,
        [ProtoEnum]
        TradeLongOnly = 138,
        [ProtoEnum]
        TradeTooManyReq = 139,
        [ProtoEnum]
        TradeAccepted = 140,
        [ProtoEnum]
        TradeUserCancel = 141,
        [ProtoEnum]
        TradeModifyDenied = 142,
        [ProtoEnum]
        TradeExpirationDenied = 143,
        [ProtoEnum]
        TradeTooManyOrders = 144,
        [ProtoEnum]
        TradeHedgeProhibited = 145,
    }
}
