using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum MarginCalcMode
    {
        [ProtoEnum]
        Forex = 0,
        [ProtoEnum]
        Cfd = 1,
        [ProtoEnum]
        Futures = 2,
        [ProtoEnum]
        CfdIndex = 3,
        [ProtoEnum]
        CfdLeverage = 4
    }
}
