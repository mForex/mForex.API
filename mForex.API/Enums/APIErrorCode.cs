using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum APIErrorCode
    {
        [ProtoEnum]
        OK = 0,
        [ProtoEnum]
        ServerError = 1,
        [ProtoEnum]
        UndefinedError = 2,
        [ProtoEnum]
        TradeError = 3,
    }
}
