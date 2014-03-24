using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum MarginMode
    {
        [ProtoEnum]
        DontUse = 0,
        [ProtoEnum]
        UseAll = 1,
        [ProtoEnum]
        UseProfit = 2,
        [ProtoEnum]
        UseLoss = 3,
    }
}
