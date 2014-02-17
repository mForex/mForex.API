using ProtoBuf;
namespace mForex.API
{
    [ProtoContract]
    public enum TradeCommand
    {
        [ProtoEnum]
        Buy = 0,
        [ProtoEnum]
        Sell = 1,
        [ProtoEnum]
        BuyLimit = 2,
        [ProtoEnum]
        SellLimit = 3,
        [ProtoEnum]
        BuyStop = 4,
        [ProtoEnum]
        SellStop = 5,
        [ProtoEnum]
        Balance = 6,
        [ProtoEnum]
        Credit = 7,
    }
}