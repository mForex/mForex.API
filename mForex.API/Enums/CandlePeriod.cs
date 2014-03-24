using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum CandlePeriod
    {
        [ProtoEnum]
        M1 = 1,
        [ProtoEnum]
        M5 = 5,
        [ProtoEnum]
        M15 = 15,
        [ProtoEnum]
        M30 = 30,
        [ProtoEnum]
        H1 = 60,
        [ProtoEnum]
        H4 = 240,
        [ProtoEnum]
        D1 = 1440,
        [ProtoEnum]
        W1 = 10080,
        [ProtoEnum]
        MN1 = 43200,
    }
}
