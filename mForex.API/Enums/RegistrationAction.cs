using ProtoBuf;

namespace mForex.API
{
    [ProtoContract]
    public enum RegistrationAction
    {
        [ProtoEnum]
        Unregister = 0,
        [ProtoEnum]
        Register = 1,
    }
}
