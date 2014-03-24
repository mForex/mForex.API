using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum AccountType
    {
        [ProtoEnum]
        Mini = 0,
        [ProtoEnum]
        Standard = 1,
        [ProtoEnum]
        Vip = 2,
    }
}
