using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum CommissionLots
    {
        [ProtoEnum]
        PerLot = 0,
        [ProtoEnum]        
        PerDeal = 1,
    }
}
