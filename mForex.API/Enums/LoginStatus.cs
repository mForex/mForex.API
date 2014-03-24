using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum LoginStatus
    {
        [ProtoEnum]
        Successful = 0,
        [ProtoEnum]
        InvalidPassword = 1,
        [ProtoEnum]
        ApiNotEnabled = 2,
        [ProtoEnum]
        InvalidProtocolVersion = 3,
    }
}
