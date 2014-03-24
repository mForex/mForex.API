using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum ProfitCalcMode
    {
        [ProtoEnum]
        Forex = 0,
        [ProtoEnum]
        Cfd = 1,
        [ProtoEnum]
        Futures = 2
    }
}
