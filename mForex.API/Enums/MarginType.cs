using ProtoBuf;


namespace mForex.API
{
    [ProtoContract]
    public enum MarginType
    {
        [ProtoEnum]
        Percent = 0,
        [ProtoEnum]
        Currency = 1,
    }
}
