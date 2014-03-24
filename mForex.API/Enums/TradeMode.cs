using ProtoBuf;

namespace mForex.API
{    
    [ProtoContract]
    public enum TradeMode
    {
        [ProtoEnum]
        No = 0,
        [ProtoEnum]
        Close = 1,
        [ProtoEnum]
        Full = 2
    }
}
