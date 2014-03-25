using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum CommissionType
    {
        [ProtoEnum]
        Money = 0,
        [ProtoEnum]        
        Pips = 1,
        [ProtoEnum]
        Percent = 2
    }
}
