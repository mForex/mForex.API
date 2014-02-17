using ProtoBuf;
namespace mForex.API
{
    [ProtoContract]
    public enum TransactionType
    {
        [ProtoEnum]
        OpenOrder = 0,
        [ProtoEnum]
        OrderClose = 2,
        [ProtoEnum]
        OrderModify = 3,
        [ProtoEnum]
        OrderDelete = 4,
    }
}