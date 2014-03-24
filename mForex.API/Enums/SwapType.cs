using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum SwapType
    {
        [ProtoEnum]
        Points = 0,
        [ProtoEnum]
        Dollars = 1,
        [ProtoEnum]
        Interest = 2,
        [ProtoEnum]
        MarginCurrency = 3
    }
}
